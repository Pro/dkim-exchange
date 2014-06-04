using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Configuration.DkimSigner.GitHub
{
    class ApiWrapper
    {
        public static Release getNewestRelease(bool includePrerelease = false)
        {
            string json = Api.MakeRequest(Api.CreateRequest("/repos/pro/dkim-exchange/releases"));
            if (json == null)
                return null;
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Release[]));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            object objResponse = jsonSerializer.ReadObject(stream);
            Release[] releases = objResponse as Release[];

            // find max version

            Release newest = null;


            foreach (Release r in releases)
            {
                if (r.Draft || (!includePrerelease && r.Prerelease))
                    continue;
                // check for valid version string
                Match match = Regex.Match(r.TagName, @"v?((?:\d+\.){0,3}\d+)",RegexOptions.IgnoreCase);

                if (!match.Success)
                    continue;
                string vStr = match.Groups[1].Value;
                r.Version = new Version(vStr);
                if (newest == null)
                    newest = r;
                else if (newest.Version < r.Version)
                    newest = r;
            }

            return newest;
        }
    }
}
