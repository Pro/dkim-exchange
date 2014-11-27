using ConfigurationSettings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// Signs MIME messages according to the DKIM standard.
    /// </summary>
    public class DkimSigner
    {
        /// <summary>
        /// The sentinel for a header separator.
        /// </summary>
        private static readonly byte[] HeaderSeparatorSentinel = Encoding.ASCII.GetBytes("\r\n\r\n");

        /// <summary>
        /// A value indicating whether or not the instance of the signer has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The headers that should be a part of the DKIM signature, if present in the message.
        /// </summary>
        private HashSet<string> eligibleHeaders;

        /// <summary>
        /// The hash algorithm that is to be employed.
        /// </summary>
        private HashAlgorithm hashAlgorithm;

        /// <summary>
        /// The DKIM code for the hash algorithm that is to be employed.
        /// </summary>
        private string hashAlgorithmDkimCode;

        /// <summary>
        /// The crypto provider code for the hash algorithm that is to be employed.
        /// </summary>
        private string hashAlgorithmCryptoCode;

        /// <summary>
        /// The DKIM canonicalization algorithm that is to be employed for the header.
        /// </summary>
        private DkimCanonicalizationKind headerCanonicalization;

        /// <summary>
        /// The DKIM canonicalization algorithm that is to be employed for the header.
        /// </summary>
        private DkimCanonicalizationKind bodyCanonicalization;

        private List<DomainElement> domains;

        /// <summary>
        /// Initializes a new instance of the <see cref="DkimSigner"/> class.
        /// </summary>
        public DkimSigner()
        {
            this.domains = new List<DomainElement>();
        }

        public void UpdateSettings(Settings config)
        {
            // Load the list of domains
            this.domains.Clear();
            foreach (DomainElement domainElement in config.Domains)
            {
                try
                {
                    try
                    {
                        if (domainElement.InitElement(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
                        {
                            this.domains.Add(domainElement);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e.Message);
                    }
                }
                catch (FileNotFoundException e)
                {
                    Logger.LogError(e.Message);
                }
            }   

            switch (config.SigningAlgorithm)
            {
                case DkimAlgorithmKind.RsaSha1:
                    this.hashAlgorithm = new SHA1CryptoServiceProvider();
                    this.hashAlgorithmCryptoCode = "SHA1";
                    this.hashAlgorithmDkimCode = "rsa-sha1";
                    break;
                case DkimAlgorithmKind.RsaSha256:
                    this.hashAlgorithm = new SHA256CryptoServiceProvider();
                    this.hashAlgorithmCryptoCode = "SHA256";
                    this.hashAlgorithmDkimCode = "rsa-sha256";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("signatureKind");
            }

            this.headerCanonicalization = config.HeaderCanonicalization;
            this.bodyCanonicalization = config.BodyCanonicalization;

            this.eligibleHeaders = new HashSet<string>();
            foreach (string headerToSign in config.HeadersToSign)
            {
                this.eligibleHeaders.Add(headerToSign.Trim());
            }

            // The From header must always be signed according to the 
            // DKIM specification.
            if (!this.eligibleHeaders.Contains("From"))
            {
                this.eligibleHeaders.Add("From");
            }
        }

        public List<DomainElement> GetDomains()
        {
            return this.domains;
        }

        /// <summary>
        /// Returns a value indicating whether or not the unsigned MIME message in the
        /// given stream can be signed. In this case, we iterate until we see the From:
        /// header, and then we only sign it if our domain matches the domain of the From:
        /// address.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>The output stream.</returns>
        public string CanSign(DomainElement domain, Stream inputStream)
        {
            if (this.disposed)
                throw new ObjectDisposedException("Exchange DkimSigner disposed.");

            inputStream.Seek(0, SeekOrigin.Begin);

            // Generate the hash for the body
            var bodyHash = this.GetBodyHash(inputStream);
            var unsignedDkimHeader = this.GetUnsignedDkimHeader(domain, bodyHash);

            // Generate the hash for the header
            var canonicalizedHeaders = this.GetCanonicalizedHeaders(inputStream);
            var signedDkimHeader = this.GetSignedDkimHeader(domain, unsignedDkimHeader, canonicalizedHeaders);

            return signedDkimHeader;
        }

        /// <summary>
        /// Writes the message containing the signed DKIM-Signature header to the output stream.
        /// </summary>
        /// <param name="inputBytes">The bytes from the stream containing the original MIME message.</param>
        /// <param name="outputStream">The stream containing the output MIME message.</param>
        /// <param name="signedDkimHeader">The signed DKIM-Signature header.</param>
        public void Sign(byte[] inputBytes, Stream outputStream, string signedDkimHeader)
        {
            if (this.disposed)
                throw new ObjectDisposedException("DomainKeysSigner");

            if (outputStream == null)
                throw new ArgumentNullException("outputStream");

            byte[] headerBuffer = Encoding.ASCII.GetBytes(signedDkimHeader);
            outputStream.Write(headerBuffer, 0, headerBuffer.Length);
            outputStream.Write(inputBytes, 0, inputBytes.Length);
        }

        /// <summary>
        /// Tests to see if the candidate sequence exists at the given position in the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="candidate">The candidate we're looking for.</param>
        /// <returns>Whether or not the candidate exists at the current position in the stream.</returns>
        private static bool IsMatch(Stream stream, byte[] candidate)
        {
            if (candidate.Length > (stream.Length - stream.Position))
                return false;

            for (int i = 0; i < candidate.Length; ++i)
                if (stream.ReadByte() != candidate[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Computes the hash of the body of the MIME message (represented by the given stream)
        /// and returned as a base64-encoded string. The stream position is reset back to the start
        /// of the stream.
        /// </summary>
        /// <param name="stream">The stream that represents the MIME message.</param>
        /// <returns>The base64-encoded hash of the body.</returns>
        private string GetBodyHash(Stream stream)
        {
            byte[] bodyBytes;
            string bodyText;
            string hashText;
            long index;

            index = -1;

            stream.Seek(0, SeekOrigin.Begin);

            while (stream.Position < stream.Length)
            {
                if (IsMatch(stream, HeaderSeparatorSentinel))
                {
                    index = stream.Position;
                    break;
                }
            }

            if (index < 0)
            {
                throw new ArgumentException("The stream did not have a MIME body.", "stream");
            }

            bodyText = new StreamReader(stream).ReadToEnd();

            if (this.bodyCanonicalization == DkimCanonicalizationKind.Relaxed)
            {
                string tempText = bodyText;
                bodyText = "";

                // Reduces all sequences of WSP within a line to a single SP
                // character.
                // Ignores all whitespace at the end of lines.  Implementations MUST
                // NOT remove the CRLF at the end of the line.

                foreach (string line in Regex.Split(tempText, @"\r?\n|\r"))
                {
                    string temp = CompactWhitespaces(line);
                    temp += "\r\n";
                    bodyText += temp;
                }
            }

            // We have to ignore all empty lines at the end of the message body.
            bodyText = Regex.Replace(bodyText, "(\r?\n)*$", string.Empty);
            bodyText += "\r\n";

            bodyBytes = Encoding.ASCII.GetBytes(bodyText);
            hashText = Convert.ToBase64String(this.hashAlgorithm.ComputeHash(bodyBytes));
            stream.Seek(0, SeekOrigin.Begin);

            return hashText;
        }

        /// <summary>
        /// Runs through the MIME stream and sucks out the headers that should be part
        /// of the DKIM signature. The headers here have their whitespace preserved and
        /// all terminate with CRLF. The stream position is returned to the start of the
        /// stream.
        /// </summary>
        /// <param name="stream">The stream containing the MIME message.</param>
        /// <returns>An enumerable of the headers that should be a part of the signature.</returns>
        private IEnumerable<string> GetCanonicalizedHeaders(Stream stream)
        {
            Dictionary<string, string> headerNameToLineMap;
            string line;
            StreamReader reader;

            stream.Seek(0, SeekOrigin.Begin);

            headerNameToLineMap = new Dictionary<string, string>();
            reader = new StreamReader(stream);

            line = reader.ReadLine();
            while (line != null)
            {
                string header;
                string[] headerParts;

                // We've reached the end of the headers (headers are
                // separated from the body by a blank line).
                if (line.Length == 0)
                    break;

                // Read a line. Because a header can be continued onto
                // subsequent lines, we have to keep reading lines until we
                // run into the end-of-headers marker (an empty line) or another
                // line that doesn't begin with a whitespace character.
                header = line + "\r\n";
                line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line) &&
                    (line.StartsWith("\t", StringComparison.Ordinal) ||
                    line.StartsWith(" ", StringComparison.Ordinal)))
                {
                    header += line + "\r\n";
                    line = reader.ReadLine();
                }

                // Extract the name of the header. Then store the full header
                // in the dictionary. We do this because DKIM mandates that we
                // only sign the LAST instance of any header that occurs.
                headerParts = header.Split(new char[] { ':' }, 2);
                if (headerParts.Length == 2)
                {
                    string headerName;

                    headerName = headerParts[0];

                    // We only want to sign the header if we were told to sign it!
                    if (this.eligibleHeaders.Contains(headerName, StringComparer.OrdinalIgnoreCase))
                    {
                        if (this.headerCanonicalization == DkimCanonicalizationKind.Relaxed)
                        {
                            // Unfold all header field continuation lines as described in
                            // [RFC5322]; in particular, lines with terminators embedded in
                            // continued header field values (that is, CRLF sequences followed by
                            // WSP) MUST be interpreted without the CRLF.  Implementations MUST
                            // NOT remove the CRLF at the end of the header field value.
                            // Delete all WSP characters at the end of each unfolded header field
                            // value.
                            // Convert all sequences of one or more WSP characters to a single SP
                            // character.  WSP characters here include those before and after a
                            // line folding boundary.

                            header = CompactWhitespaces(header);
                            header += "\r\n";

                            // Delete any WSP characters remaining before and after the colon
                            // separating the header field name from the header field value.  The
                            // colon separator MUST be retained.
                            // We can't simply replace all " : " with ":" because only the first
                            // colon should be changed. If e.g. the subject header contains a
                            // "RE: TEST" it mustn't be replaced with "RE:TEST"
                            int firstPos = header.IndexOf(':');
                            if (firstPos > 0 && firstPos < header.Length - 2)
                            {
                                // colon found. Now remove any spaces before/after the colon

                                //check how many whitespaces are before the colon
                                int beforeCount = 0;
                                for (int i = firstPos - 1; i >= 0; i--)
                                {
                                    if (header[i] == ' ')
                                        beforeCount++;
                                    else
                                        break;
                                }
                                if (beforeCount > 0)
                                {
                                    //now remove them
                                    header = header.Remove(firstPos - beforeCount, beforeCount);
                                }
                                // colon is now at another position
                                firstPos -= beforeCount;

                                //check how many whitespaces are after the colon
                                int afterCount = 0;
                                for (int i = firstPos + 1; i < header.Length; i++)
                                {
                                    if (header[i] == ' ')
                                        afterCount++;
                                    else
                                        break;
                                }
                                if (afterCount > 0)
                                {
                                    //now remove them
                                    header = header.Remove(firstPos + 1, afterCount);
                                }
                            }

                            // Convert all header field names (not the header field values) to
                            // lowercase.  For example, convert "SUBJect: AbC" to "subject: AbC".
                            string[] temp = header.Split(new char[] { ':' }, 2);
                            header = temp[0].ToLower() + ":" + temp[1];
                        }

                        headerNameToLineMap[headerName] = header;
                    }
                }
            }

            stream.Seek(0, SeekOrigin.Begin);

            return headerNameToLineMap.Values.OrderBy(x => x, StringComparer.Ordinal);
        }

        /// <summary>
        /// Gets the version of the DKIM-Signature header with the signature appended, along with
        /// the CRLF.
        /// </summary>
        /// <param name="unsignedDkimHeader">The unsigned DKIM header, to use as a template.</param>
        /// <param name="canonicalizedHeaders">The headers to be included as part of the signature.</param>
        /// <returns>The signed DKIM-Signature header.</returns>
        private string GetSignedDkimHeader(DomainElement domain, string unsignedDkimHeader, IEnumerable<string> canonicalizedHeaders)
        {
            byte[] signatureBytes;
            string signatureText;
            StringBuilder signedDkimHeader;

            if (domain.CryptoProvider == null)
                throw new Exception("CryptoProvider for domain " + domain.Domain + " is null.");

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    foreach (var canonicalizedHeader in canonicalizedHeaders)
                        writer.Write(canonicalizedHeader);

                    if (this.headerCanonicalization == DkimCanonicalizationKind.Relaxed)
                    {
                        unsignedDkimHeader = Regex.Replace(unsignedDkimHeader, @" ?: ?", ":");
                        string[] temp = unsignedDkimHeader.Split(new char[] { ':' }, 2);
                        unsignedDkimHeader = temp[0].ToLower() + ":" + temp[1];
                    }

                    writer.Write(unsignedDkimHeader);
                    writer.Flush();

                    stream.Seek(0, SeekOrigin.Begin);

                    // Why not pass this.hashAlgorithm here, since we already have it? If we're supporting
                    // Exchange 2007, then we're stuck on CLR 2.0. The SHA-256 functionality was added in
                    // .NET 3.5 SP1, but it was done in such a way that the switch statement used internally
                    // by the Crypto .NET classes won't recognize the new SHA256CryptoServiceProvider type.
                    // So, we have to use the string method instead. More details available at
                    // http://blogs.msdn.com/b/shawnfa/archive/2008/08/25/using-rsacryptoserviceprovider-for-rsa-sha256-signatures.aspx
                    signatureBytes = domain.CryptoProvider.SignData(stream, this.hashAlgorithmCryptoCode);
                }
            }

            signatureText = Convert.ToBase64String(signatureBytes);
            signedDkimHeader = new StringBuilder(unsignedDkimHeader.Substring(0, unsignedDkimHeader.Length - 1));
            signedDkimHeader.Append(signatureText);
            signedDkimHeader.Append(";\r\n");

            return signedDkimHeader.ToString();
        }

        /// <summary>
        /// Builds an unsigned DKIM-Signature header. Note that the returned
        /// header will NOT have a CRLF at the end.
        /// </summary>
        /// <param name="bodyHash">The hash of the body.</param>
        /// <returns>The unsigned DKIM-Signature header.</returns>
        private string GetUnsignedDkimHeader(DomainElement domain, string bodyHash)
        {
            return string.Format(
                    CultureInfo.InvariantCulture,
                    "DKIM-Signature: v=1; a={0}; s={1}; d={2}; c={3}/{4}; q=dns/txt; h={5}; bh={6}; b=;",
                    this.hashAlgorithmDkimCode,
                    domain.Selector,
                    domain.Domain,
                    this.headerCanonicalization.ToString().ToLower(),
                    this.bodyCanonicalization.ToString().ToLower(),
                    string.Join(" : ", this.eligibleHeaders.OrderBy(x => x, StringComparer.Ordinal).ToArray()),
                    bodyHash);
        }

        /// <summary>
        /// Remove extra white spaces
        /// http://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
        /// </summary>
        /// <param name="s">A string</param>
        /// <returns>The convert string</returns>
        private static String CompactWhitespaces(String s)
        {
            StringBuilder sb = new StringBuilder(s);
            CompactWhitespaces(sb);
            return sb.ToString();
        }

        /// <summary>
        /// Remove extra white spaces
        /// http://stackoverflow.com/questions/6442421/c-sharp-fastest-way-to-remove-extra-white-spaces
        /// </summary>
        /// <param name="sb">A string</param>
        private static void CompactWhitespaces(StringBuilder sb)
        {
            if (sb.Length == 0)
                return;

            // set [start] to first not-whitespace char or to sb.Length

            int start = 0;

            while (start < sb.Length)
            {
                if (Char.IsWhiteSpace(sb[start]))
                    start++;
                else
                    break;
            }

            // if [sb] has only whitespaces, then return empty string

            if (start == sb.Length)
            {
                sb.Length = 0;
                return;
            }

            // set [end] to last not-whitespace char

            int end = sb.Length - 1;

            while (end >= 0)
            {
                if (Char.IsWhiteSpace(sb[end]))
                    end--;
                else
                    break;
            }

            // compact string

            int dest = 0;
            bool previousIsWhitespace = false;

            for (int i = 0; i <= end; i++)
            {
                if (Char.IsWhiteSpace(sb[i]))
                {
                    if (!previousIsWhitespace)
                    {
                        previousIsWhitespace = true;
                        sb[dest] = ' ';
                        dest++;
                    }
                }
                else
                {
                    previousIsWhitespace = false;
                    sb[dest] = sb[i];
                    dest++;
                }
            }

            sb.Length = dest;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DkimSigner"/> class.
        /// </summary>
        ~DkimSigner()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
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
                if (this.hashAlgorithm != null)
                {
                    this.hashAlgorithm.Clear();
                    this.hashAlgorithm = null;
                }
            }

            this.disposed = true;
        }
    }
}