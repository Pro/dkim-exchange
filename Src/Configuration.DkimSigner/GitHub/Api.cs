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
            HttpWebResponse oResponse = null;
            oRequest.UserAgent = ".NET Framework API Client";

            string sResult = null;
            try
            {
                oResponse = (HttpWebResponse) oRequest.GetResponse();

                if (oResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = oResponse.GetResponseStream();
                    if (responseStream == null)
                    {
                        return null;
                    }
                    sResult = new StreamReader(responseStream).ReadToEnd();
                }
                //else
                //{
                //    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", oResponse.StatusCode, oResponse.StatusDescription));
                //}
            }
            catch (Exception)
            {
                sResult = null;
            }

            if (oResponse != null)
            {
                oResponse.Dispose();
            }

            return sResult;
        }
    }
}
