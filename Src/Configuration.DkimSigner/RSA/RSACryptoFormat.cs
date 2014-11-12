namespace DkimSigner.RSA
{
    /// <summary>
    /// Enumeration of the kinds of RSA key format
    /// </summary>
    public enum RSACryptoFormat : int
    {
        /// <summary>
        /// PEM-encoded RSA key
        /// </summary>
        PEM,

        /// <summary>
        /// DER-encoded RSA key
        /// </summary>
        DER,

        /// <summary>
        /// XML-encoded RSA key
        /// </summary>
        XML,

        /// <summary>
        /// UNKNOWN-encoded RSA key
        /// </summary>
        UNKNOWN
    }
}
