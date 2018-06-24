using Exchange.Dkim;
using Heijden.DNS;
using MimeKit;
using MimeKit.Cryptography;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Org.BouncyCastle.Security;

namespace Exchange.DkimVerificator
{
    class PublicKeyLocator : IDkimPublicKeyLocator
    {

        public PublicKeyLocator()
        {
        }

        public AsymmetricKeyParameter LocatePublicKey(string methods, string domain, string selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!methods.Equals("dns/txt"))
            {
                Logger.LogWarning("DKIM DNS query method not supported: " + methods + ". Must be 'dns/txt'");
                return null;
            }

            string txtRecord = getTxtRecord(domain, selector);

            txtRecord = txtRecord.Replace(" ", "").TrimEnd(';');
            Dictionary<string, string> dict = txtRecord.Split(';').Select(x => x.Split('=')).ToDictionary(x => x[0], x => x[1]);

            // k must be rsa
            if (dict.ContainsKey("k"))
            {
                if (!dict["k"].Equals("rsa"))
                {
                    Logger.LogWarning("DKIM DNS record invalid. k must be 'rsa' but is: " + dict["k"]);
                    return null;
                }
            }

            if (!dict.ContainsKey("p"))
            {
                Logger.LogWarning("DKIM DNS record invalid. There is no 'p' value in:" + txtRecord);
                return null;
            }

            try
            {
                byte[] byteKey = Convert.FromBase64String(dict["p"]);
                return PublicKeyFactory.CreateKey(byteKey);
            }
            catch (Exception e)
            {
                Logger.LogWarning("DKIM DNS record invalid. Could not convert public key of '" + txtRecord + "' Error: " + e.Message);
            }

            return null;
        }

        private string getTxtRecord(string domain, string selector)
        {
            string sFullDomain = selector + "._domainkey." + domain;

            try
            {
                Resolver oResolver = new Resolver();
                oResolver.Recursion = true;
                oResolver.UseCache = true;

                // Get the TXT record for DKIM
                Response oResponse = oResolver.Query(sFullDomain, QType.TXT, QClass.IN);
                if (oResponse.RecordsTXT.GetLength(0) > 0)
                {
                    RecordTXT oTxtRecord = oResponse.RecordsTXT[0];
                    return string.Join(string.Empty, oTxtRecord.TXT.ToArray());
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Coldn't get DNS record for " + sFullDomain + ": " + ex.Message);
            }
            return null;
        }
    }
}
