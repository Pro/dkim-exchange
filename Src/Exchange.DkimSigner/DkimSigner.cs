using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Exchange.DkimSigner.Configuration;
using Exchange.DkimSigner.Helper;
using Microsoft.Exchange.Data.Transport;
using MimeKit;
using MimeKit.Cryptography;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// Signs MIME messages according to the DKIM standard.
    /// </summary>
    public class DkimSigner
    {

        /// <summary>
        /// A value indicating whether or not the instance of the signer has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The headers that should be a part of the DKIM signature, if present in the message.
        /// </summary>
        private HeaderId[] eligibleHeaders;

        /// <summary>
        /// The hash algorithm that is to be employed.
        /// </summary>
        private HashAlgorithm hashAlgorithm;

        /// <summary>
        /// The DKIM canonicalization algorithm that is to be employed for the header.
        /// </summary>
        private DkimCanonicalizationAlgorithm headerCanonicalization;

        /// <summary>
        /// The DKIM canonicalization algorithm that is to be employed for the header.
        /// </summary>
        private DkimCanonicalizationAlgorithm bodyCanonicalization;

        /// <summary>
        /// Map the domain Host part to the corresponding domain settings object
        /// </summary>
        private Dictionary<string, DomainElementSigner> domains;

        /// <summary>
        /// Object used as a mutex when settings are updated during execution
        /// </summary>
        private object settingsMutex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigner"/> class.
        /// </summary>
        public DkimSigner()
        {
            domains = new Dictionary<string, DomainElementSigner>(StringComparer.OrdinalIgnoreCase);
            settingsMutex = new object();
        }


#if EX_2007_SP3 || EX_2010 || EX_2010_SP1 || EX_2010_SP2 || EX_2010_SP3
        /// <summary>
        /// Parses a string header to its corresponding enum value. Only needed for .NET 3.5 since it is natively supported in 4.0 and above.
        /// </summary>
        /// <param name="strEnumValue">The string value of the enum field</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool TryParseHeader(string strEnumValue, out HeaderId parsedHeaderId)
        {
            parsedHeaderId = HeaderId.Unknown;
            if (!Enum.IsDefined(typeof(HeaderId), strEnumValue))
                return false;

            parsedHeaderId = (HeaderId) Enum.Parse(typeof (HeaderId), strEnumValue);
            return true;
        }
#endif

        public void UpdateSettings(Settings config)
        {
            lock (settingsMutex)
            {
                // Load the list of domains
                domains.Clear();

                DkimSignatureAlgorithm signatureAlgorithm;
                
                switch (config.SigningAlgorithm)
                {
                    case DkimAlgorithmKind.RsaSha1:
                        signatureAlgorithm = DkimSignatureAlgorithm.RsaSha1;
                        break;
                    case DkimAlgorithmKind.RsaSha256:
                        signatureAlgorithm = DkimSignatureAlgorithm.RsaSha256;
                        break;
                    default:
                        // ReSharper disable once NotResolvedInText
                        throw new ArgumentOutOfRangeException("config.SigningAlgorithm");
                }


                foreach (DomainElement domainElement in config.Domains)
                {
                    string privateKey = domainElement.PrivateKeyPathAbsolute(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    if (String.IsNullOrEmpty(privateKey) || !File.Exists(privateKey))
                    {
                        Logger.LogError("The private key for domain " + domainElement.Domain + " wasn't found: " + privateKey + ". Ignoring domain.");

                    }
                    
                    //check if the private key can be parsed
                    try
                    {
                        KeyHelper.ParseKeyPair(privateKey);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Couldn't load private key for domain " + domainElement.Domain + ": " + ex.Message);
                        continue;
                    }

                    MimeKit.Cryptography.DkimSigner signer;
                    try
                    {
                        signer = new MimeKit.Cryptography.DkimSigner(KeyHelper.ParsePrivateKey(privateKey), domainElement.Domain,
                            domainElement.Selector)
                        {
                            SignatureAlgorithm = signatureAlgorithm
                        };
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Could not initialize MimeKit DkimSigner for domain " + domainElement.Domain + ": " + ex.Message);
                        continue;
                    }
                    domains.Add(domainElement.Domain, new DomainElementSigner(domainElement, signer));
                }

                headerCanonicalization = config.HeaderCanonicalization == DkimCanonicalizationKind.Relaxed ? DkimCanonicalizationAlgorithm.Relaxed : DkimCanonicalizationAlgorithm.Simple;
                
                bodyCanonicalization = config.BodyCanonicalization == DkimCanonicalizationKind.Relaxed ? DkimCanonicalizationAlgorithm.Relaxed : DkimCanonicalizationAlgorithm.Simple;

                List<HeaderId> headerList = new List<HeaderId>();
                foreach (string headerToSign in config.HeadersToSign)
                {
                    HeaderId headerId;
#if EX_2007_SP3 || EX_2010 || EX_2010_SP1 || EX_2010_SP2 || EX_2010_SP3
                    if (!TryParseHeader(headerToSign, out headerId))
#else
                    if (!Enum.TryParse(headerToSign, true, out headerId))
#endif
                    {
                        Logger.LogWarning("Invalid value for header to sign: '" + headerToSign + "'. This header will be ignored.");
                    }
                    headerList.Add(headerId);
                }

                // The From header must always be signed according to the 
                // DKIM specification.
                if (!headerList.Contains(HeaderId.From))
                {
                    headerList.Add(HeaderId.From);
                }
                eligibleHeaders = headerList.ToArray();
            }
        }

        public Dictionary<string, DomainElementSigner> GetDomains()
        {
            lock (settingsMutex)
            {
                return domains;
            }
        }

        /// <summary>
        /// Signs the given mail item using the provided signer. The mailItem object will be updated so that it includes the signature.
        /// </summary>
        /// <param name="domainSigner">The domain and its signer</param>
        /// <param name="mailItem">The mail item to sign</param>
        /// <returns></returns>
        public void SignMessage(DomainElementSigner domainSigner, MailItem mailItem)
        {
            if (disposed)
                throw new ObjectDisposedException("DkimSigner");

            using (Stream stream = mailItem.GetMimeReadStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                
                Logger.LogDebug("Parsing the MimeMessage");
                MimeMessage message = MimeMessage.Load(stream, true);

                Logger.LogDebug("Signing the message");
                lock (settingsMutex)
                {
                    message.Sign(domainSigner.Signer, eligibleHeaders, headerCanonicalization, bodyCanonicalization);
                }
                var value = message.Headers[HeaderId.DkimSignature];
                Logger.LogDebug("Got signing header: " + value);

                // we first need to create a memory stream, because if WriteTo is called with mailItem Stream directly, it throws an exception somehow.
                MemoryStream memoryOutputStream = new MemoryStream();
                message.WriteTo(FormatOptions.Default, memoryOutputStream);
                memoryOutputStream.Seek(0, SeekOrigin.Begin);

                using (Stream outputStream = mailItem.GetMimeWriteStream())
                {
                    memoryOutputStream.WriteTo(outputStream);
                    outputStream.Close();
                }

                stream.Close();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DkimSigner"/> class.
        /// </summary>
        ~DkimSigner()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (hashAlgorithm != null)
                {
                    hashAlgorithm.Clear();
                    hashAlgorithm = null;
                }
            }

            disposed = true;
        }
    }
}
