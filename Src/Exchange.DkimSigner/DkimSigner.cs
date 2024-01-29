using Exchange.DkimSigner.Configuration;
using Exchange.DkimSigner.Helper;
using Microsoft.Exchange.Data.Transport;
using MimeKit;
using MimeKit.Cryptography;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Exchange.DkimSigner
{
	/// <summary>
	/// Signs MIME messages according to the DKIM standard.
	/// </summary>
	public class DkimSigner
	{

		/// <summary>
		/// The headers that should be a part of the DKIM signature, if present in the message.
		/// </summary>
		private HeaderId[] eligibleHeaders;

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
		private readonly Dictionary<string, DomainElementSigner> domains;

		/// <summary>
		/// Object used as a mutex when settings are updated during execution
		/// </summary>
		private readonly object settingsMutex;

		/// <summary>
		/// Initializes a new instance of the <see cref="DkimSigner"/> class.
		/// </summary>
		public DkimSigner()
		{
			domains = new Dictionary<string, DomainElementSigner>(StringComparer.OrdinalIgnoreCase);
			settingsMutex = new object();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Log general exceptions")]
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

				bodyCanonicalization = config.BodyCanonicalization == DkimCanonicalizationKind.Relaxed ? DkimCanonicalizationAlgorithm.Relaxed : DkimCanonicalizationAlgorithm.Simple;
				headerCanonicalization = config.HeaderCanonicalization == DkimCanonicalizationKind.Relaxed ? DkimCanonicalizationAlgorithm.Relaxed : DkimCanonicalizationAlgorithm.Simple;

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
						AsymmetricKeyParameter key = KeyHelper.ParsePrivateKey(privateKey);

						signer = new MimeKit.Cryptography.DkimSigner(key, domainElement.Domain, domainElement.Selector, signatureAlgorithm)
						{
							BodyCanonicalizationAlgorithm = bodyCanonicalization,
							HeaderCanonicalizationAlgorithm = headerCanonicalization
						};
					}
					catch (Exception ex)
					{
						Logger.LogError("Could not initialize MimeKit DkimSigner for domain " + domainElement.Domain + ": " + ex.Message);
						continue;
					}
					domains.Add(domainElement.Domain, new DomainElementSigner(domainElement, signer));
				}

				List<HeaderId> headerList = new List<HeaderId>();
				foreach (string headerToSign in config.HeadersToSign)
				{
					if (!Enum.TryParse(headerToSign, true, out HeaderId headerId) || (headerId == HeaderId.Unknown))
					{
						Logger.LogWarning("Invalid value for header to sign: '" + headerToSign + "'. This header will be ignored.");
					}
					headerList.Add(headerId);
				}

				// The From header must always be signed according to the DKIM specification.
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
			// MailItem.GetMimeWriteStream() internally uses
			// Microsoft.Exchange.Data.Mime.MimeDocument.GetLoadStream(), which may reformat the
			// message using different formatting than is originally read from
			// MailItem.GetMimeReadStream().  To prevent these formatting changes from invalidating
			// the DKIM signature, we must read then write then re-read the message to ensure that
			// any formatting changes are made before we sign the message.
			using (MemoryStream memStream = new MemoryStream())
			{
				using (Stream inputStream = mailItem.GetMimeReadStream())
				{
					inputStream.Seek(0, SeekOrigin.Begin);
					inputStream.CopyTo(memStream);
				}
				memStream.Seek(0, SeekOrigin.Begin);
				using (Stream outputStream = mailItem.GetMimeWriteStream())
				{
					memStream.WriteTo(outputStream);
				}
			}

			using (Stream inputStream = mailItem.GetMimeReadStream())
			{
				inputStream.Seek(0, SeekOrigin.Begin);
				if (Logger.IsDebugEnabled())
				{
					Logger.LogDebug("Parsing the MimeMessage");
				}

				MimeMessage message = MimeMessage.Load(inputStream, true);
				// 'inputStream' cannot be disposed until we are done with 'message'

				if (Logger.IsDebugEnabled())
				{
					Logger.LogDebug("Signing the message");
				}

				lock (settingsMutex)
				{
					domainSigner.Signer.Sign(message, eligibleHeaders);
				}
				var value = message.Headers[HeaderId.DkimSignature];
				
				if (Logger.IsDebugEnabled())
				{
					Logger.LogDebug("Got signing header: " + value);
				}

				// The Stream returned by mailItem.GetMimeWriteStream() will throw an exception if
				// Stream.Write() is called after Stream.Flush() has been called, but
				// MimeMessage.WriteTo(FormatOptions, Stream) may call Stream.Flush() before the full
				// message has been written.  To avoid exceptions we must buffer the message in a
				// MemoryStream.
				using (MemoryStream memStream = new MemoryStream())
				{
					message.WriteTo(FormatOptions.Default, memStream);
					memStream.Seek(0, SeekOrigin.Begin);
					using (Stream outputStream = mailItem.GetMimeWriteStream())
					{
						memStream.WriteTo(outputStream);
					}
				}
			}
		}
	}
}