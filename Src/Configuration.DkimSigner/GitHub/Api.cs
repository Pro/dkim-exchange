using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Configuration.DkimSigner.GitHub
{
    class Api
    {
        public static string CreateRequest(string queryString)
        {
            string UrlRequest = "https://api.github.com" +
                                 queryString;
            return (UrlRequest);
        }

        public static string MakeRequest(string requestUrl)
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            request.UserAgent = ".NET Framework API Client";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }

    }
}
