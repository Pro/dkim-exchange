namespace Exchange.DkimSigner.Configuration
{
    public class DomainElementSigner
    {
        public DomainElement DomainElement { get; set; }
        public MimeKit.Cryptography.DkimSigner Signer { get; set; }

        public DomainElementSigner(DomainElement domain, MimeKit.Cryptography.DkimSigner signerInstance)
        {
            DomainElement = domain;
            Signer = signerInstance;
        }
    }
}
