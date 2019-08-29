namespace Exchange.DkimSigner.Configuration
{
    /// <summary>
    /// Enumeration of the kinds of signature and hashing algorithms 
    /// that can be used with DKIM.
    /// </summary>
    public enum DkimAlgorithmKind
    {
        /// <summary>
        /// The RSA SHA-1 hashing algorithm should be used.
        /// </summary>
        RsaSha1,

        /// <summary>
        /// The RSA SHA-2 (256) hashing algorithm should be used.
        /// </summary>
        RsaSha256,

        /// <summary>
        /// The Ed25519-SHA256 hashing algorithm should be used.
        /// </summary>
        Ed25519SHA256,
    }
}
