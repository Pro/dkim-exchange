using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Exchange.DkimConfiguration.GitHub
{
    public class ApiWrapper
    {
        public static List<Release> GetAllRelease(bool bIncludePrerelease = false, Version oMinimalVersion = null)
        {
            List<Release> aoRelease = null;

            string sJson = Api.MakeRequest(Api.CreateRequest("/repos/pro/dkim-exchange/releases"));

            if (sJson != null)
            {
                DataContractJsonSerializer oJsonSerializer = new DataContractJsonSerializer(typeof(Release[]));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(sJson));
                
                object objResponse = oJsonSerializer.ReadObject(stream);
                Release[] oTemp = (Release[]) objResponse;

                aoRelease = new List<Release>(oTemp);

                foreach (Release oRelease in aoRelease.ToList())
                {
                    if (oRelease.Draft || (!bIncludePrerelease && oRelease.Prerelease))
                    {
                        aoRelease.Remove(oRelease);
                        continue;
                    }

                    // Check for valid version string
                    Match oMatch = Regex.Match(oRelease.TagName, @"v?((?:\d+\.){0,3}\d+)",RegexOptions.IgnoreCase);
                    if (!oMatch.Success)
                    {
                        aoRelease.Remove(oRelease);
                    }
                    else
                    {
                        string sVersion = oMatch.Groups[1].Value;
                        oRelease.Version = new Version(sVersion);

                        if (oMinimalVersion != null && oRelease.Version < oMinimalVersion)
                        {
                            aoRelease.Remove(oRelease);
                        }
                    }
                }
            }

            return aoRelease;
        }

        public static Release GetNewestRelease(bool bIncludePrerelease = false, Version oMinimalVersion = null)
        {
            List<Release> aoRelease = GetAllRelease(bIncludePrerelease, oMinimalVersion);
            if (aoRelease == null)
                return null;
            Release oNewestRelease = null;

            foreach (Release oRelease in aoRelease)
            {
                if (oNewestRelease == null || oNewestRelease.Version < oRelease.Version)
                {
                    oNewestRelease = oRelease;
                }
            }

            return oNewestRelease;
        }
    }
}