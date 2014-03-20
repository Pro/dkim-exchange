namespace Exchange.DkimSigner
{
    using ConfigurationSettings;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Signs MIME messages according to the DKIM standard.
    /// </summary>
    public class DefaultDkimSigner : IDkimSigner
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

        /// <summary>
        /// The list of domains loaded from config file.
        /// </summary>
        private List<DomainElement> domainSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDkimSigner"/> class.
        /// </summary>
        /// <param name="signatureKind">The signature kind to use.</param>
        /// <param name="headersToSign">The headers to be signed. If null, only the From header will be signed.</param>
        /// <param name="selector">The domain containing the public key TXT record.</param>
        /// <param name="domain">The DNS selector.</param>
        /// <param name="encodedKey">The PEM-encoded key.</param>
        public DefaultDkimSigner(
            DkimAlgorithmKind signatureKind,
            DkimCanonicalizationKind headerCanonicalizationKind,
            DkimCanonicalizationKind bodyCanonicalizationKind,
            IEnumerable<string> headersToSign,
            List<DomainElement> domainSettings)
        {
            this.domainSettings = domainSettings;
        
            switch (signatureKind)
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

            this.headerCanonicalization = headerCanonicalizationKind;

            this.bodyCanonicalization = bodyCanonicalizationKind;

            this.eligibleHeaders = new HashSet<string>();

            if (headersToSign != null)
            {
                foreach (var headerToSign in headersToSign)
                {
                    this.eligibleHeaders.Add(headerToSign.Trim());
                }
            }

            // The From header must always be signed according to the 
            // DKIM specification.
            this.eligibleHeaders.Add("From");
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DefaultDkimSigner"/> class.
        /// </summary>
        ~DefaultDkimSigner()
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
        /// Returns a value indicating whether or not the unsigned MIME message in the
        /// given stream can be signed. In this case, we iterate until we see the From:
        /// header, and then we only sign it if our domain matches the domain of the From:
        /// address.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>The output stream.</returns>
        public string CanSign(Stream inputStream)
        {
            if (domainSettings == null || domainSettings.Count == 0)
                return "";

            string line = string.Empty;
            StreamReader reader;

            if (this.disposed)
            {
                throw new ObjectDisposedException("DomainKeysSigner");
            }

            if (inputStream == null)
            {
                throw new ArgumentNullException("inputStream");
            }

            reader = new StreamReader(inputStream);

            inputStream.Seek(0, SeekOrigin.Begin);

            DomainElement domainFound = null;

            System.Collections.ArrayList toDomains = new System.Collections.ArrayList();
            MailAddress from = null;

            line = reader.ReadLine();
            while (line != null)
            {
                string header;
                string[] headerParts;

                // We've reached the end of the headers (headers are
                // separated from the body by a blank line).
                if (line.Length == 0)
                {
                    break;
                }

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

                    if (headerName.Equals("From", StringComparison.OrdinalIgnoreCase))
                    {
                        // We don't break here because we want to read the bottom-most
                        // instance of the From: header (there should be only one, but
                        // if there are multiple, it's the last one that matters).
                        
                        foreach (DomainElement e in domainSettings) {
                            if (header
                                .ToUpperInvariant()
                                .Contains("@" + e.Domain.ToUpperInvariant())) {
                                domainFound = e;
                            }
                        }
                        try
                        {
                            from = new MailAddress(headerParts[1].ToLowerInvariant());
                        }
                        catch (System.FormatException ex)
                        {
                            Logger.LogError("Couldn't parse from address: '" + headerParts[1].ToLowerInvariant() + "': " + ex.Message + ". Ignoring sender rule");
                        }
                    }
                    
                    if (headerName.Equals("To", StringComparison.OrdinalIgnoreCase)) {
                        // check each To header and add it to the collection
                        toDomains.Add(headerParts[1].ToLowerInvariant());
                    }
                }
            }

            inputStream.Seek(0, SeekOrigin.Begin);
            if (domainFound == null)
            {
                return "";
            }
            
            if (from!=null && !Regex.Match(from.Address, domainFound.SenderRule).Success)
            {
                Logger.LogInformation("Skipping '" + from.Address + "' because sender rule '" + domainFound.SenderRule + "' not matched");
                return "";
            }

            bool ruleMatch = false;

            foreach (string to in toDomains) {
                MailAddress addr = null;
                try
                {
                    addr = new MailAddress(to);
                    ruleMatch |= Regex.Match(addr.Address, domainFound.RecipientRule).Success;
                }
                catch (System.FormatException ex)
                {
                    Logger.LogError("Couldn't parse to address: '" + to + "': " + ex.Message + ". Ignoring recipient rule");
                    ruleMatch = true;
                }
                if (ruleMatch)
                    break;
            }

            if (!ruleMatch) {
                Logger.LogInformation("Skipping '" + domainFound.Domain + "' because rule '" + domainFound.RecipientRule + "' not matched");
                return "";
            }

            // Generate the hash for the body
            var bodyHash = this.GetBodyHash(inputStream);
            var unsignedDkimHeader = this.GetUnsignedDkimHeader(bodyHash, domainFound);

            // Generate the hash for the header
            var canonicalizedHeaders = this.GetCanonicalizedHeaders(inputStream);
            var signedDkimHeader = this.GetSignedDkimHeader(unsignedDkimHeader, canonicalizedHeaders, domainFound);

            return signedDkimHeader;
        }

        public string SourceMessage(Stream inputStream)
        {
            string bodyText;

            inputStream.Seek(0, SeekOrigin.Begin);

            bodyText = new StreamReader(inputStream).ReadToEnd();

            inputStream.Seek(0, SeekOrigin.Begin);

            return bodyText;
        }

        /// <summary>
        /// Writes a signed version of the unsigned MIME message in the input stream
        /// to the output stream.
        /// </summary>
        /// <param name="inputStream">The input stream byte data.</param>
        /// <param name="outputStream">The output stream.</param>
        public void Sign(byte[] inputBytes, Stream outputStream, string signeddkim)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("DomainKeysSigner");
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }

            WriteSignedMimeMessage(inputBytes, outputStream, signeddkim);
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
            {
                return false;
            }

            for (int i = 0; i < candidate.Length; ++i)
            {
                if (stream.ReadByte() != candidate[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Writes the message containing the signed DKIM-Signature header to the output stream.
        /// </summary>
        /// <param name="input">The bytes from the stream containing the original MIME message.</param>
        /// <param name="output">The stream containing the output MIME message.</param>
        /// <param name="signedDkimHeader">The signed DKIM-Signature header.</param>
        private static void WriteSignedMimeMessage(byte[] inputBytes, Stream output, string signedDkimHeader)
        {
            byte[] headerBuffer;

            headerBuffer = Encoding.ASCII.GetBytes(signedDkimHeader);
            output.Write(headerBuffer, 0, headerBuffer.Length);

            output.Write(inputBytes, 0, inputBytes.Length);
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
                throw new ArgumentException(
                    "The stream did not have a MIME body.",
                    "stream");
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
                {
                    break;
                }

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
                            header = Regex.Replace(header, @" ?: ?", ":");

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
        private string GetSignedDkimHeader(string unsignedDkimHeader, IEnumerable<string> canonicalizedHeaders, DomainElement domain)
        {
            byte[] signatureBytes;
            string signatureText;
            StringBuilder signedDkimHeader;

            if (domain.CryptoProvider == null)
            {
                throw new Exception("CryptoProvider for domain " + domain.Domain + " is null.");
            }

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    foreach (var canonicalizedHeader in canonicalizedHeaders)
                    {
                        writer.Write(canonicalizedHeader);
                    }

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
        private string GetUnsignedDkimHeader(string bodyHash, DomainElement domain)
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
        public static String CompactWhitespaces(String s)
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
        public static void CompactWhitespaces(StringBuilder sb)
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
    }
}
