using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Exchange.DkimSigner.Helper
{
    public class KeyHelper
    {
        /// <summary>
        /// Parses the private key from given file name.
        /// If the key file contains a key pair, it will be returned directly.
        /// If the key file only contains the RSA private key the public key will be created from the modulus and exponent.
        /// </summary>
        /// <param name="privateKeyFile">path to the private key file</param>
        /// <returns>the parsed key pair</returns>
        /// <exception cref="FormatException">Thrown if the key is not valid or can't be parsed</exception>
        public static AsymmetricCipherKeyPair ParseKeyPair(string privateKeyFile)
        {
            object obj = ReadPem(privateKeyFile);
            if (obj == null)
                throw new FormatException("The key file has an invalid PEM format. " + privateKeyFile);

            if (obj is AsymmetricCipherKeyPair)
            {
                return obj as AsymmetricCipherKeyPair;
            }
            if (obj is RsaPrivateCrtKeyParameters)
            {
                RsaPrivateCrtKeyParameters rsaPrivateKey = obj as RsaPrivateCrtKeyParameters;

                var publicParameters = new RsaKeyParameters(false, rsaPrivateKey.Modulus,rsaPrivateKey.PublicExponent);
                
                return new AsymmetricCipherKeyPair(publicParameters, rsaPrivateKey);

            }

            throw new FormatException("The given key does not have the correct type. The keyfile must include the private and public key in PEM format. It is of type: " +
                                      obj.GetType());

        }

        /// <summary>
        /// Reads the PEM key file and returns the object.
        /// </summary>
        /// <param name="fileName">Path to the pem file</param>
        /// <returns>the read object which may be of different key types</returns>
        /// <exception cref="FormatException">Thrown if the key is not in PEM format</exception>
        private static object ReadPem(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The key file does not exist: " + fileName);

            using (StreamReader file = new StreamReader(fileName))
            {
                PemReader pRd = new PemReader(file);

                object obj = pRd.ReadObject();
                pRd.Reader.Close();
                if (obj == null)
                {
                    throw new FormatException("The key file " + fileName + " is no valid PEM format");
                }
                return obj;
            }
        }

        /// <summary>
        /// Parses the public key from the given key file.
        /// If the key file contains a key pair, only its public key will be returned.
        /// If the key file only contains the RSA private key the public key will be created from the modulus and exponent.
        /// </summary>
        /// <param name="keyFile">path to the public key file</param>
        /// <returns>the parsed public key parameters</returns>
        /// <exception cref="FormatException">Thrown if the key is not in PEM format</exception>
        public static AsymmetricKeyParameter ParsePublicKey(string keyFile)
        {
            object obj = ReadPem(keyFile);
            if (obj == null)
                throw new FormatException("The key file has an invalid PEM format. " + keyFile);

            if (obj is RsaPrivateCrtKeyParameters)
            {
                RsaPrivateCrtKeyParameters rsaPrivateKey = obj as RsaPrivateCrtKeyParameters;

                return new RsaKeyParameters(false, rsaPrivateKey.Modulus, rsaPrivateKey.PublicExponent);
            }
            if (obj is AsymmetricKeyParameter)
            {

                AsymmetricKeyParameter key = obj as AsymmetricKeyParameter;
                if (key.IsPrivate)
                    throw new FormatException("The given key file is a private key but a public key was expected. " + keyFile);
                return key;
            }
            if (obj is AsymmetricCipherKeyPair)
            {
                AsymmetricCipherKeyPair key = obj as AsymmetricCipherKeyPair;
                if (key.Public == null)
                    throw new FormatException("The given key file was successfully parsed but didn't contain any public key. " + keyFile);
                return key.Public;
            }
            throw new FormatException("The given key does not have the correct type. It is of type: " +
                                      obj.GetType());
        }
    }
}
