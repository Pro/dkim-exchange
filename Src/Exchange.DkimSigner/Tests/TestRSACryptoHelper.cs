using System;
using System.IO;
using NUnit.Framework;

using DkimSigner.RSA;

namespace Exchange.DkimSigner.Tests.Ressources
{
    [TestFixture]
    public class TestRSACryptoHelper
    {
        [Test]
        public void TestGetFormatFromEncodedRsaPrivateKeyDER()
        {
            byte[] fileBytes = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.der");

            Assert.AreEqual(RSACryptoFormat.DER, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytes));
        }

        [Test]
        public void TestGetFormatFromEncodedRsaPrivateKeyPEM()
        {
            byte[] fileBytes = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.pem");

            Assert.AreEqual(RSACryptoFormat.PEM, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytes));
        }

        [Test]
        public void TestGetFormatFromEncodedRsaPrivateKeyXML()
        {
            byte[] fileBytes = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.xml");

            Assert.AreEqual(RSACryptoFormat.XML, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytes));
        }

        //[Test]
        //public void GetProviderFromKeyFile()
        //{
            //Assert.AreEqual(RSACryptoFormat.XML, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytes));
        //}
    }
}
