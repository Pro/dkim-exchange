using ConfigurationSettings;
using NUnit.Framework;

namespace Exchange.DkimSigner.Tests
{
    [TestFixture]
    public class TestSettings
    {
        [Test]
        public void TestInitHeadersToSign()
        {
            Settings config = new Settings();
            config.InitHeadersToSign();
            Assert.AreEqual(new[] { "From", "Subject", "To", "Date", "Message-ID" }, config.HeadersToSign);
        }

        //[Test]
        //public void TestLoad()
        //{
            //Settings configExpected = new Settings();
            //configExpected.Loglevel = 3;
            //configExpected.SigningAlgorithm = DkimAlgorithmKind.RsaSha1;
            //configExpected.HeaderCanonicalization = DkimCanonicalizationKind.Simple;
            //configExpected.BodyCanonicalization = DkimCanonicalizationKind.Simple;
            //configExpected.HeaderCanonicalization = new string[] { "From", "Subject", "To", "Date", "Message-ID" };
            //configExpected.Domains = new List

            //Settings configActual = new Settings();
            //configActual.Load();
            //Assert.AreEqual(configExpected, configActual);
        //}

        //[Test]
        //public void TestSave()
        //{
            //Settings config = new Settings();
            //config.Load();
            //Assert.AreEqual();
        //}        
    }
}