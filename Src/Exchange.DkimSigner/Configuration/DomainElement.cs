using System;
using System.IO;
using System.Security.Cryptography;
using DkimSigner.RSA;

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
        private RSACryptoServiceProvider _cryptoProvider;
        public RSACryptoServiceProvider CryptoProvider
        {
            get { return _cryptoProvider; }
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
            string path = Path.IsPathRooted(PrivateKeyFile) ? PrivateKeyFile : Path.Combine(basePath, @"keys\" + PrivateKeyFile);

            if (String.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new FileNotFoundException("The private key for domain " + Domain + " wasn't found: " + path);
            }

            try
            {
                _cryptoProvider = RSACryptoHelper.GetProviderFromKeyFile(path);
            }
            catch (Exception e)
            {
                throw new CryptographicException("Couldn't load the key '" + path + "' for domain " + Domain + ". Error message: " + e.Message);
            }
            if (_cryptoProvider == null)
            {
                throw new RSACryptoHelperException("Couldn't load the key '" + path + "' for domain " + Domain + ". Invalid key format or broken file");
            }

            return true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DkimSigner"/> class.
        /// </summary>
        ~DomainElement()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                if (_cryptoProvider != null)
                {
                    _cryptoProvider.Clear();
                    _cryptoProvider = null;
                }
            }
        }
    }
}