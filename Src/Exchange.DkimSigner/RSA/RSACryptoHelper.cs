using Configuration.DkimSigner.Properties;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DkimSigner.RSA
{
    /// <summary>
    /// Contains helper methods for retrieving encryption objects.
    /// </summary>
    public static class RSACryptoHelper
    {
        /// <summary>
        /// The header for an RSA private key in PEM format.
        /// </summary>
        private const string PemRsaPrivateKeyHeader = "-----BEGIN RSA PRIVATE KEY-----";

        /// <summary>
        /// The footer for an RSA private key in PEM format.
        /// </summary>
        private const string PemRsaPrivateKeyFooter = "-----END RSA PRIVATE KEY-----";

        public static RSACryptoFormat GetFormatFromEncodedRsaPrivateKey(byte[] encodedKey)
        {
            RSACryptoFormat format;

            using (var stream = new MemoryStream(encodedKey))
            {
                using (var reader = new BinaryReader(stream))
                {
                    try
                    {
                        // The data is read as little endian order.
                        ushort test = reader.ReadUInt16();
                        switch (test)
                        {
                            case 0x8130:
                                format = RSACryptoFormat.DER;
                                break;
                            case 0x8230:
                                format = RSACryptoFormat.DER;
                                break;
                            case 0x523C:
                                format = RSACryptoFormat.XML;
                                break;
                            case 0x2D2D:
                                format = RSACryptoFormat.PEM;
                                break;
                            default:
                                format = RSACryptoFormat.UNKNOWN;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(Resources.RSACryptHelper_UnknownFormat, "encodedKey", ex);
                    }
                }
            }

            return format;
        }

        public static RSAParameters GenerateRsaParameters()
        {
            return new RSACryptoServiceProvider().ExportParameters(true);
        }

        /// <summary>
        /// Generate a RSA Private Key in XML format
        /// </summary>
        /// <returns>RSA Private Key in a byte array</returns>
        public static byte[] GenerateXMLEncodedRsaPrivateKey()
        {
            return Encoding.ASCII.GetBytes(new RSACryptoServiceProvider().ToXmlString(true));
        }

        /// <summary>
        /// Attempts to get an instance of an RSACryptoServiceProvider from a 
        /// PEM-encoded RSA private key, commonly used by OpenSSL. You would think
        /// that this would be built into the .NET framework, but oh well.
        /// </summary>
        /// <param name="encodedKey">The PEM-encoded key.</param>
        /// <returns>The RSACryptoServiceProvider instance, which the caller is
        /// responsible for disposing.</returns>
        public static RSACryptoServiceProvider GetProviderFromPemEncodedRsaPrivateKey(string encodedKey)
        {
            encodedKey = encodedKey.Trim();

            if (!encodedKey.StartsWith(PemRsaPrivateKeyHeader, StringComparison.Ordinal) ||
                !encodedKey.EndsWith(PemRsaPrivateKeyFooter, StringComparison.Ordinal))
            {
                throw new ArgumentException(Resources.RSACryptHelper_BadPemFormat, "encodedKey");
            }

            encodedKey = encodedKey.Substring(
                PemRsaPrivateKeyHeader.Length,
                encodedKey.Length - PemRsaPrivateKeyFooter.Length - PemRsaPrivateKeyHeader.Length);

            return GetProviderFromDerEncodedRsaPrivateKey(Convert.FromBase64String(encodedKey.Trim()));
        }

        /// <summary>
        /// Attempts to get an instance of an RSACryptoServiceProvider from a DER-encoded
        /// RSA private key, commonly used by OpenSSL. It's ripped pretty shamelessly from
        /// http://www.jensign.com/opensslkey/opensslkey.cs.
        /// </summary>
        /// <param name="encodedKey">The DER-encoded key.</param>
        /// <returns>The RSACryptoServiceProvider instance, which the caller is
        /// responsible for disposing.</returns>
        public static RSACryptoServiceProvider GetProviderFromDerEncodedRsaPrivateKey(byte[] encodedKey)
        {
            RSACryptoServiceProvider provider;

            using (var stream = new MemoryStream(encodedKey))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var count = 0;

                    try
                    {
                        // The data is read as little endian order.
                        switch (reader.ReadUInt16())
                        {
                            case 0x8130:
                                // Advance 1 byte.
                                reader.ReadByte();
                                break;
                            case 0x8230:
                                // Advance 2 bytes.
                                reader.ReadInt16();
                                break;
                            default:
                                return null;
                        }

                        if (reader.ReadUInt16() != 0x102)
                        {
                            // Unsupported version number; we don't know
                            // how to read anything else.
                            return null;
                        }

                        if (reader.ReadByte() != 0)
                        {
                            return null;
                        }

                        count = ReadFieldLength(reader);
                        var modulus = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var exponent = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var d = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var p = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var q = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var dp = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var dq = reader.ReadBytes(count);

                        count = ReadFieldLength(reader);
                        var inverseQ = reader.ReadBytes(count);

                        var parameters = new RSAParameters();
                        parameters.Modulus = modulus;
                        parameters.Exponent = exponent;
                        parameters.D = d;
                        parameters.P = p;
                        parameters.Q = q;
                        parameters.DP = dp;
                        parameters.DQ = dq;
                        parameters.InverseQ = inverseQ;

                        provider = new RSACryptoServiceProvider();
                        provider.ImportParameters(parameters);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(Resources.RSACryptHelper_BadDerFormat, "encodedKey", ex);
                    }
                }
            }

            return provider;
        }

        /// <summary>
        /// Attempts to get an instance of an RSACryptoServiceProvider from a 
        /// XML-encoded RSA private key, commonly used by OpenSSL. You would think
        /// that this would be built into the .NET framework, but oh well.
        /// </summary>
        /// <param name="encodedKey">The XML-encoded key.</param>
        /// <returns>The RSACryptoServiceProvider instance, which the caller is
        /// responsible for disposing.</returns>
        public static RSACryptoServiceProvider GetProviderFromXmlEncodedRsaPrivateKey(string encodedKey)
        {
            encodedKey = encodedKey.Trim();

            RSACryptoServiceProvider provider;

            try
            {
                provider = new RSACryptoServiceProvider();
                provider.FromXmlString(encodedKey);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(Resources.RSACryptHelper_BadXmlFormat, "encodedKey", ex);
            }

            return provider;
        }

        /// <summary>
        /// Gets the size of the next field in a DER-encoded RSA private key.
        /// This is ripped shamelessly from http://www.jensign.com/opensslkey/opensslkey.cs
        /// </summary>
        /// <param name="reader">The reader containing the key data.</param>
        /// <returns>The length of the next RSA key field, in bytes.</returns>
        private static int ReadFieldLength(BinaryReader reader)
        {
            byte bt;    // The value of the last read byte
            int count;  // The length of the next field, in bytes.

            bt = 0;
            count = 0;

            if (reader.ReadByte() != 0x02)
            {
                // We expect an integer.
                return 0;
            }

            bt = reader.ReadByte();

            switch (bt)
            {
                case 0x81:
                    {
                        // Data size is in the next byte.
                        count = reader.ReadByte();
                    }

                    break;

                case 0x82:
                    {
                        // Data size is in the next two bytes.
                        var high = reader.ReadByte();
                        var low = reader.ReadByte();
                        count = BitConverter.ToInt32(
                            new byte[] { low, high, 0, 0 },
                            0);
                        break;
                    }

                default:
                    {
                        // We already have the data size.
                        count = bt;
                    }

                    break;
            }

            // Remove high-order zeroes in the data.
            while (reader.ReadByte() == 0)
            {
                count -= 1;
            }

            // The last read byte wasn't a zero, so back up a byte
            // for the next invocation of this function.
            reader.BaseStream.Seek(-1, SeekOrigin.Current);

            return count;
        }
    }
}