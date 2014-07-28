using System;
using System.IO;
using System.Net;

namespace Configuration.DkimSigner.GitHub
{
    public class Api
    {
        public static string CreateRequest(string sQuery)
        {
            return "https://api.github.com" + sQuery;
        }

        public static string MakeRequest(string sUrl)
        {
            HttpWebRequest oRequest = (HttpWebRequest) WebRequest.Create(sUrl);
            oRequest.UserAgent = ".NET Framework API Client";

            using (HttpWebResponse oResponse = (HttpWebResponse) oRequest.GetResponse())
            {
                if (oResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", oResponse.StatusCode, oResponse.StatusDescription));
                }

                return new StreamReader(oResponse.GetResponseStream()).ReadToEnd();
            }
        }

    }
}
