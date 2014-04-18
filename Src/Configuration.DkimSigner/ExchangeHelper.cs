using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.DkimSigner
{
    public class ExchangeHelper
    {
        public static string checkExchangeVersionInstalled()
        {
            try
            {
                string domain = Domain.GetCurrentDomain().ToString();
                DirectoryEntry rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/rootDSE", domain));
                DirectoryEntry objDirectoryEntry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", domain, rootDSE.Properties["configurationNamingContext"].Value.ToString()));
                DirectorySearcher searcher = new DirectorySearcher(objDirectoryEntry, "(&(objectClass=msExchExchangeServer))");
                SearchResultCollection col = searcher.FindAll();
                string version = string.Empty;
                foreach (SearchResult result in col)
                {
                    DirectoryEntry user = result.GetDirectoryEntry();
                    if (String.Equals(user.Properties["name"].Value.ToString(), Dns.GetHostName(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        version = user.Properties["serialNumber"].Value.ToString();
                        break;
                    }
                }

                return version;
            }
            catch (Exception)
            {
                return "Not installed";
            }
        }
    }
}
