using System;
using System.IO;
using System.Security.Cryptography;
using NUnit.Framework;

using DkimSigner.RSA;

namespace Exchange.DkimSigner.Tests.Ressources
{
    [TestFixture]
    public class TestRSACryptoHelper
    {
        [Test]
        public void TestGetFormatFromEncodedRsaPrivateKey()
        {
            byte[] fileBytesDER = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.der");
            byte[] fileBytesPEM = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.pem");
            byte[] fileBytesXML = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.xml");

            Assert.AreEqual(RSACryptoFormat.DER, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytesDER));
            Assert.AreEqual(RSACryptoFormat.PEM, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytesPEM));
            Assert.AreEqual(RSACryptoFormat.XML, RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(fileBytesXML));
        }

        [Test]
        public void GetProviderFromKeyFile()
        {
            string keyValid;
            string keyDER = null;
            string keyPEM = null;
            string keyXML = null;

            RSAParameters parameters = new RSAParameters();
            parameters.D = Convert.FromBase64String(@"pRgiRK2tfvFdYcGbiqyJ+rgi/HTAPEnR/dtr87I5ctDwOzBG0qOaB3oiUW7qEU0G0iy4hNc1zaHsjhSZYgKZEHP+Xgs7RJZYOTPI9sqbymrDJDLur7h2pMvsqLhcJjEn6qz+hnLMT046D9uSMg9Tpr0Z6FUiOoAwnUZcSK50gj0=");
            parameters.DP = Convert.FromBase64String(@"w49jS+lsTPP5l8QLmMWeyKQ1PzWpRWsV0DJPHRZFHdjNtQkW1zMn5yJsGJ0a9yqXROv6n3BY18iuqY0S/c2PYw==");
            parameters.DQ = Convert.FromBase64String(@"tzis4VqnqbIsZ/CkcBE6Nz3/Rk9nnU5Bw6JyZMs2DVY3JJtOVdmqzZmQ4KquonW5IH6ti3W3844ao+sHm3o3xQ==");
            parameters.Exponent = Convert.FromBase64String(@"AQAB");
            parameters.InverseQ = Convert.FromBase64String(@"/ihX2MBotgMyCIhbyR8l/7G877/nF5BFIC8RGUJqh0SFovYRHVEleLTK7Pi7eA7+OokKEwshZlfwPuE7xiIKyw==");
            parameters.Modulus = Convert.FromBase64String(@"9ws/iSH6l00F/3HUhoJQyY2Y1iorw0roP9BcZRxEmtEfRzPmLnWwrQpusWJUfK71LQu/OLPD9qtnfQdVIGBMns7gfFJBGq+Dsef7CVRb0HIZv3kqUAh8AI46KIx3xRKsdVY4mh7QZcSdAyHHUi0839yNVbObhXDUNETgT5CzKFU=");
            parameters.P = Convert.FromBase64String(@"/u0mEbvml8X8DrbKBiB0QGX9+G2ALRN+SwasDi7jW65SeBf49ENPxH8iC5XXB/yxQpBV2RojferhdE4Nh1+btw==");
            parameters.Q = Convert.FromBase64String(@"+BWZ2QG6x2gL5qOqfZd6wtP/eQRLVz9OC2IUfw0ZojHuWXt45ybw/F+o+bQmyQcTFYES6hFYTUtWjMgn5IG0Uw==");

            using (RSACryptoServiceProvider providerValid = new RSACryptoServiceProvider())
            {
                providerValid.ImportParameters(parameters);
                keyValid = providerValid.ToXmlString(true);
            }

            using (RSACryptoServiceProvider providerDER = RSACryptoHelper.GetProviderFromKeyFile(@"..\..\..\..\Resources\Tests\private.der"))
            {
                if (providerDER != null)
                    keyDER = providerDER.ToXmlString(true);
            }

            using (RSACryptoServiceProvider providerPEM = RSACryptoHelper.GetProviderFromKeyFile(@"..\..\..\..\Resources\Tests\private.pem"))
            {
                if (providerPEM != null)
                    keyPEM = providerPEM.ToXmlString(true);
            }

            using (RSACryptoServiceProvider providerXML = RSACryptoHelper.GetProviderFromKeyFile(@"..\..\..\..\Resources\Tests\private.xml"))
            {
                if (providerXML != null)
                    keyXML = providerXML.ToXmlString(true);
            }

            Assert.AreEqual(keyValid, keyDER);
            Assert.AreEqual(keyValid, keyPEM);
            Assert.AreEqual(keyValid, keyXML);
        }

        [Test]
        public void GetProviderFromXmlEncodedRsaPrivateKey()
        {
            string keyValid;
            string keyXML = null;
            
            RSAParameters parameters = new RSAParameters();
            parameters.D = Convert.FromBase64String(@"pRgiRK2tfvFdYcGbiqyJ+rgi/HTAPEnR/dtr87I5ctDwOzBG0qOaB3oiUW7qEU0G0iy4hNc1zaHsjhSZYgKZEHP+Xgs7RJZYOTPI9sqbymrDJDLur7h2pMvsqLhcJjEn6qz+hnLMT046D9uSMg9Tpr0Z6FUiOoAwnUZcSK50gj0=");
            parameters.DP = Convert.FromBase64String(@"w49jS+lsTPP5l8QLmMWeyKQ1PzWpRWsV0DJPHRZFHdjNtQkW1zMn5yJsGJ0a9yqXROv6n3BY18iuqY0S/c2PYw==");
            parameters.DQ = Convert.FromBase64String(@"tzis4VqnqbIsZ/CkcBE6Nz3/Rk9nnU5Bw6JyZMs2DVY3JJtOVdmqzZmQ4KquonW5IH6ti3W3844ao+sHm3o3xQ==");
            parameters.Exponent = Convert.FromBase64String(@"AQAB");
            parameters.InverseQ = Convert.FromBase64String(@"/ihX2MBotgMyCIhbyR8l/7G877/nF5BFIC8RGUJqh0SFovYRHVEleLTK7Pi7eA7+OokKEwshZlfwPuE7xiIKyw==");
            parameters.Modulus = Convert.FromBase64String(@"9ws/iSH6l00F/3HUhoJQyY2Y1iorw0roP9BcZRxEmtEfRzPmLnWwrQpusWJUfK71LQu/OLPD9qtnfQdVIGBMns7gfFJBGq+Dsef7CVRb0HIZv3kqUAh8AI46KIx3xRKsdVY4mh7QZcSdAyHHUi0839yNVbObhXDUNETgT5CzKFU=");
            parameters.P = Convert.FromBase64String(@"/u0mEbvml8X8DrbKBiB0QGX9+G2ALRN+SwasDi7jW65SeBf49ENPxH8iC5XXB/yxQpBV2RojferhdE4Nh1+btw==");
            parameters.Q = Convert.FromBase64String(@"+BWZ2QG6x2gL5qOqfZd6wtP/eQRLVz9OC2IUfw0ZojHuWXt45ybw/F+o+bQmyQcTFYES6hFYTUtWjMgn5IG0Uw==");

            using (RSACryptoServiceProvider providerValid = new RSACryptoServiceProvider())
            {
                providerValid.ImportParameters(parameters);
                keyValid = providerValid.ToXmlString(true);
            }

            byte[] fileBytes = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.xml");
            using (RSACryptoServiceProvider providerXML = RSACryptoHelper.GetProviderFromXmlEncodedRsaPrivateKey(System.Text.Encoding.ASCII.GetString(fileBytes).Trim(), "test-private"))
            {
                if (providerXML != null)
                    keyXML = providerXML.ToXmlString(true);
            }

            Assert.AreEqual(keyValid, keyXML);
        }

        [Test]
        public void GetProviderFromPemEncodedRsaPrivateKey()
        {
            string keyValid;
            string keyPEM = null;

            RSAParameters parameters = new RSAParameters();
            parameters.D = Convert.FromBase64String(@"pRgiRK2tfvFdYcGbiqyJ+rgi/HTAPEnR/dtr87I5ctDwOzBG0qOaB3oiUW7qEU0G0iy4hNc1zaHsjhSZYgKZEHP+Xgs7RJZYOTPI9sqbymrDJDLur7h2pMvsqLhcJjEn6qz+hnLMT046D9uSMg9Tpr0Z6FUiOoAwnUZcSK50gj0=");
            parameters.DP = Convert.FromBase64String(@"w49jS+lsTPP5l8QLmMWeyKQ1PzWpRWsV0DJPHRZFHdjNtQkW1zMn5yJsGJ0a9yqXROv6n3BY18iuqY0S/c2PYw==");
            parameters.DQ = Convert.FromBase64String(@"tzis4VqnqbIsZ/CkcBE6Nz3/Rk9nnU5Bw6JyZMs2DVY3JJtOVdmqzZmQ4KquonW5IH6ti3W3844ao+sHm3o3xQ==");
            parameters.Exponent = Convert.FromBase64String(@"AQAB");
            parameters.InverseQ = Convert.FromBase64String(@"/ihX2MBotgMyCIhbyR8l/7G877/nF5BFIC8RGUJqh0SFovYRHVEleLTK7Pi7eA7+OokKEwshZlfwPuE7xiIKyw==");
            parameters.Modulus = Convert.FromBase64String(@"9ws/iSH6l00F/3HUhoJQyY2Y1iorw0roP9BcZRxEmtEfRzPmLnWwrQpusWJUfK71LQu/OLPD9qtnfQdVIGBMns7gfFJBGq+Dsef7CVRb0HIZv3kqUAh8AI46KIx3xRKsdVY4mh7QZcSdAyHHUi0839yNVbObhXDUNETgT5CzKFU=");
            parameters.P = Convert.FromBase64String(@"/u0mEbvml8X8DrbKBiB0QGX9+G2ALRN+SwasDi7jW65SeBf49ENPxH8iC5XXB/yxQpBV2RojferhdE4Nh1+btw==");
            parameters.Q = Convert.FromBase64String(@"+BWZ2QG6x2gL5qOqfZd6wtP/eQRLVz9OC2IUfw0ZojHuWXt45ybw/F+o+bQmyQcTFYES6hFYTUtWjMgn5IG0Uw==");

            using (RSACryptoServiceProvider providerValid = new RSACryptoServiceProvider())
            {
                providerValid.ImportParameters(parameters);
                keyValid = providerValid.ToXmlString(true);
            }

            byte[] fileBytes = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.pem");
            using (RSACryptoServiceProvider providerPEM = RSACryptoHelper.GetProviderFromPemEncodedRsaPrivateKey(System.Text.Encoding.ASCII.GetString(fileBytes).Trim(), "test-private"))
            {
                if (providerPEM != null)
                    keyPEM = providerPEM.ToXmlString(true);
            }

            Assert.AreEqual(keyValid, keyPEM);
        }

        [Test]
        public void GetProviderFromDerEncodedRsaPrivateKey()
        {
            string keyValid;
            string keyDER = null;

            RSAParameters parameters = new RSAParameters();
            parameters.D = Convert.FromBase64String(@"pRgiRK2tfvFdYcGbiqyJ+rgi/HTAPEnR/dtr87I5ctDwOzBG0qOaB3oiUW7qEU0G0iy4hNc1zaHsjhSZYgKZEHP+Xgs7RJZYOTPI9sqbymrDJDLur7h2pMvsqLhcJjEn6qz+hnLMT046D9uSMg9Tpr0Z6FUiOoAwnUZcSK50gj0=");
            parameters.DP = Convert.FromBase64String(@"w49jS+lsTPP5l8QLmMWeyKQ1PzWpRWsV0DJPHRZFHdjNtQkW1zMn5yJsGJ0a9yqXROv6n3BY18iuqY0S/c2PYw==");
            parameters.DQ = Convert.FromBase64String(@"tzis4VqnqbIsZ/CkcBE6Nz3/Rk9nnU5Bw6JyZMs2DVY3JJtOVdmqzZmQ4KquonW5IH6ti3W3844ao+sHm3o3xQ==");
            parameters.Exponent = Convert.FromBase64String(@"AQAB");
            parameters.InverseQ = Convert.FromBase64String(@"/ihX2MBotgMyCIhbyR8l/7G877/nF5BFIC8RGUJqh0SFovYRHVEleLTK7Pi7eA7+OokKEwshZlfwPuE7xiIKyw==");
            parameters.Modulus = Convert.FromBase64String(@"9ws/iSH6l00F/3HUhoJQyY2Y1iorw0roP9BcZRxEmtEfRzPmLnWwrQpusWJUfK71LQu/OLPD9qtnfQdVIGBMns7gfFJBGq+Dsef7CVRb0HIZv3kqUAh8AI46KIx3xRKsdVY4mh7QZcSdAyHHUi0839yNVbObhXDUNETgT5CzKFU=");
            parameters.P = Convert.FromBase64String(@"/u0mEbvml8X8DrbKBiB0QGX9+G2ALRN+SwasDi7jW65SeBf49ENPxH8iC5XXB/yxQpBV2RojferhdE4Nh1+btw==");
            parameters.Q = Convert.FromBase64String(@"+BWZ2QG6x2gL5qOqfZd6wtP/eQRLVz9OC2IUfw0ZojHuWXt45ybw/F+o+bQmyQcTFYES6hFYTUtWjMgn5IG0Uw==");

            using (RSACryptoServiceProvider providerValid = new RSACryptoServiceProvider())
            {
                providerValid.ImportParameters(parameters);
                keyValid = providerValid.ToXmlString(true);
            }

            byte[] fileBytes = File.ReadAllBytes(@"..\..\..\..\Resources\Tests\private.der");
            using (RSACryptoServiceProvider providerDER = RSACryptoHelper.GetProviderFromDerEncodedRsaPrivateKey(fileBytes, "test-private"))
            {
                if (providerDER != null)
                    keyDER = providerDER.ToXmlString(true);
            }

            Assert.AreEqual(keyValid, keyDER);
        }
    }
}