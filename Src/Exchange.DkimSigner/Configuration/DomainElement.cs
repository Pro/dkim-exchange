using System.IO;

namespace Exchange.DkimSigner.Configuration
{
    public class DomainElement
    {
        public string Domain { get; set; }
        public string Selector { get; set; }
        public string PrivateKeyFile { get; set; }

        /// <summary>
        /// Domain element constructor
        /// </summary>
        public DomainElement() { }

        public DomainElement(string domain, string selector, string privateKeyFile)
        {
            Domain = domain;
            Selector = selector;
            PrivateKeyFile = privateKeyFile;
        }

        public override string ToString()
        {
            return Domain;
        }

        public string PrivateKeyPathAbsolute(string basePath)
        {
            return Path.IsPathRooted(PrivateKeyFile) ? PrivateKeyFile : Path.Combine(basePath, @"keys\" + PrivateKeyFile);
        }

    }
}