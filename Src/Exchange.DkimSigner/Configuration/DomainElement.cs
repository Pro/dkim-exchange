using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConfigurationSettings
{
    public class DomainElement
    {
        public string Domain { get; set; }
        public string Selector { get; set; }
        public string PrivateKeyFile { get; set; }

        /// <summary>
        /// RSACryptoServiceProvider to manipulate to encrypt the information
        /// </summary>
        private RSACryptoServiceProvider cryptoProvider;
        public RSACryptoServiceProvider CryptoProvider
        {
            get { return cryptoProvider; }
        }

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

        /// <summary>
        /// Create the RSACryptoServiceProvider for the domain
        /// </summary>
        /// <param name="basePath">Path of the private key to open</param>
        /// <returns></returns>
        public bool InitElement(string basePath)
        {
            string path = Path.IsPathRooted(PrivateKeyFile) ? @"keys\" + PrivateKeyFile : Path.Combine(basePath, @"keys\" + PrivateKeyFile);

            if (String.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new FileNotFoundException("The private key for domain " + Domain + " not found: " + path);
            }

            try
            {
                string xmlkey = File.ReadAllText(path, Encoding.ASCII).Trim();
                cryptoProvider = new RSACryptoServiceProvider();
                cryptoProvider.FromXmlString(xmlkey);                    
            }
            catch(Exception)
            {
                throw new CryptographicException("The format of the private key for domain " + Domain + " isn't valid. The private key should be in a XML format.");
            }

            return true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DkimSigner"/> class.
        /// </summary>
        ~DomainElement()
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
                if (this.cryptoProvider != null)
                {
                    this.cryptoProvider.Clear();
                    this.cryptoProvider = null;
                }
            }
        }
    }
}