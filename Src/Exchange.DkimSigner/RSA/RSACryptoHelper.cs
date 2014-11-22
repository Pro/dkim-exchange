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
        /// The header for an RSA private key in OpenSSL PEM format.
        /// </summary>
        private const string PemSSLPrivateKeyHeader = "-----BEGIN RSA PRIVATE KEY-----";

        /// <summary>
        /// The footer for an RSA private key in OpenSSL PEM format.
        /// </summary>
        private const string PemSSLPrivateKeyFooter = "-----END RSA PRIVATE KEY-----";


        /// <summary>
        /// The header for an RSA private key in PKCS #8 PEM format.
        /// </summary>
        private const string PemP8PrivateKeyHeader = "-----BEGIN PRIVATE KEY-----";

        /// <summary>
        /// The footer for an RSA private key in PKCS #8 PEM format.
        /// </summary>
        private const string PemP8PrivateKeyFooter = "-----END PRIVATE KEY-----";

        /// <summary>
        /// Detects the RSACryptoFormat from the given byte encoded private key.
        /// </summary>
        /// <param name="encodedKey"></param>
        /// <returns></returns>
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
                        throw new RSACryptoHelperException("Unknown key format. (" + ex.Message + ")", "encodedKey", ex);
                    }
                }
            }

            return format;
        }

        /// <summary>
        /// Reads the given private key file and parses the containing key into a RSACryptoServiceProvider.
        /// Supported formats are XML, PEM, DER.
        /// 
        /// Throws a RSACryptoHelperException if the key couldn't be loaded.
        /// </summary>
        /// <param name="pathToFile">Path to the key file</param>
        /// <returns>the parsed key</returns>
        public static RSACryptoServiceProvider GetProviderFromKeyFile(string pathToFile)
        {
            byte[] fileBytes = File.ReadAllBytes(pathToFile);
            switch (RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytes))
            {
                case RSACryptoFormat.DER:
                    return RSACryptoHelper.GetProviderFromDerEncodedRsaPrivateKey(fileBytes);
                case RSACryptoFormat.PEM:
                    return RSACryptoHelper.GetProviderFromPemEncodedRsaPrivateKey(System.Text.Encoding.ASCII.GetString(fileBytes).Trim());
                case RSACryptoFormat.XML:
                    return RSACryptoHelper.GetProviderFromXmlEncodedRsaPrivateKey(System.Text.Encoding.ASCII.GetString(fileBytes).Trim());
                case RSACryptoFormat.UNKNOWN:
                default:
                    throw new RSACryptoHelperException("Couldn't identify key format for '" + pathToFile + "'. It should be one of the following RSA formats: XML, PEM, DER");
            }
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
                throw new RSACryptoHelperException("Invalid XML format for key. (" + ex.Message + ")", "encodedKey", ex);
            }

            return provider;
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
            
            if (encodedKey.StartsWith(PemSSLPrivateKeyHeader, StringComparison.Ordinal) &&
                encodedKey.EndsWith(PemSSLPrivateKeyFooter, StringComparison.Ordinal))
            {
                encodedKey = encodedKey.Substring(PemSSLPrivateKeyHeader.Length, encodedKey.Length - PemSSLPrivateKeyFooter.Length - PemSSLPrivateKeyHeader.Length);
                //remove any newlines
                encodedKey = encodedKey.Replace("\r", "").Replace("\n", "");

                //the encodedKey is now in base64 encoded DER format
                return GetProviderFromDerEncodedRsaPrivateKey(Convert.FromBase64String(encodedKey.Trim()));
            } else if (encodedKey.StartsWith(PemP8PrivateKeyHeader, StringComparison.Ordinal) &&
             encodedKey.EndsWith(PemP8PrivateKeyFooter, StringComparison.Ordinal))
            {
                encodedKey = encodedKey.Substring(PemP8PrivateKeyHeader.Length, encodedKey.Length - PemP8PrivateKeyFooter.Length - PemP8PrivateKeyHeader.Length);
                //remove any newlines
                encodedKey = encodedKey.Replace("\r", "").Replace("\n", "");

                //the encodedKey is now in base64 encoded PKCS8 format
                return GetProviderFromPKCS8PrivateKey(Convert.FromBase64String(encodedKey.Trim()));
            }
            else 
                throw new RSACryptoHelperException("Invalid PEM format for key. The key needs to start with '" + PemSSLPrivateKeyHeader + "' and end with '" + PemSSLPrivateKeyFooter + "' or start with '" + PemP8PrivateKeyHeader + "' and end with '" + PemP8PrivateKeyFooter + "'", "encodedKey");

        }

        /// <summary>
        /// Attempts to get an instance of an RSACryptoServiceProvider from a DER-encoded
        /// RSA private key in PKCS #8 format. It's ripped pretty shamelessly from
        /// http://www.jensign.com/opensslkey/opensslkey.cs.
        /// </summary>
        /// <param name="encodedKey">The DER-encoded key.</param>
        /// <returns>The RSACryptoServiceProvider instance, which the caller is
        /// responsible for disposing.</returns>
        public static RSACryptoServiceProvider GetProviderFromPKCS8PrivateKey(byte[] pkcs8)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            // this byte[] includes the sequence byte and terminal encoded null 
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();	//advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();	//advance 2 bytes
                else
                    return null;


                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);		//read the Sequence OID
                if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)	//expect an Octet string 
                    return null;

                bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                        binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                return GetProviderFromDerEncodedRsaPrivateKey(rsaprivkey);
            }

            catch (Exception ex)
            {
                throw new RSACryptoHelperException("Invalid PEM PKCS #8 format for key. (" + ex.Message + ")", "encodedKey", ex);
            }

            finally { binr.Close(); }

        }

        /// <summary>
        /// Compares two byte arrays and checks if they are equal
        /// It's ripped pretty shamelessly from
        /// http://www.jensign.com/opensslkey/opensslkey.cs.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
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
                        throw new RSACryptoHelperException("Invalid DER format for key. (" + ex.Message + ")", "encodedKey", ex);
                    }
                }
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
