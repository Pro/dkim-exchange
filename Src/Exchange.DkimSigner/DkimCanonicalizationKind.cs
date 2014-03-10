namespace Exchange.DkimSigner
{
    /// <summary>
    /// Enumeration of the kinds of canonicalization algorithms 
    /// that can be used with DKIM.
    /// </summary>
    public enum DkimCanonicalizationKind : int
    {
        /// <summary>
        /// The simple canonicalization algorithm should be used.
        /// </summary>
        Simple,

        /// <summary>
        /// The relaxed canonicalization algorithm should be used.
        /// </summary>
        Relaxed
    }
}
