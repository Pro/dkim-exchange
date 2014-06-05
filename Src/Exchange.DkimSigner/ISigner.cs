using ConfigurationSettings;
using System;
using System.IO;

namespace Exchange.DkimSigner
{
    /// <summary>
    /// An object that knows how to sign a MIME message according to the DKIM standard.
    /// </summary>
    public interface ISigner : IDisposable
    {
        /// <summary>
        /// Returns a value indicating whether or not the unsigned MIME message in the
        /// given stream can be signed.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <returns>The output stream.</returns>
        string CanSign(DomainElement domain, Stream inputStream);

        /// <summary>
        /// Writes a signed version of the unsigned MIME message in the input stream
        /// to the output stream.
        /// </summary>
        /// <param name="inputStream">The input stream byte data.</param>
        /// <param name="outputStream">The output stream.</param>
        void Sign(byte[] inputBytes, Stream outputStream, string signeddkim);
    }
}
