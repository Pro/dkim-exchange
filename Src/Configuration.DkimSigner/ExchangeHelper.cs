using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public class ExchangeHelper
    {
        /// <summary>
        /// Get the current Exchange version for the current server from Active Directy (ldap)
        /// </summary>
        /// <returns></returns>
        public static string checkExchangeVersionInstalled()
        {
            try
            {
                string domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
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

                return version != string.Empty ? version: "Not installed";
            }
            catch (Exception)
            {
                return "Not installed";
            }
        }

        public static bool installTransoportAgent()
        {


            // First make sure the following Registry key exists
            // HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM

            RegistryKey root = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM", false);
            if (root == null)
            {
                //doesn't exist
                Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM");
            }

            string baseDir = @"C:\Program Files\Exchange DkimSigner";

            //TODO net stop MSExchangeTransport 
            //TODO copy .dll and .config

            string hostName = System.Net.Dns.GetHostEntry("").HostName;

            PSCredential psCredential = (PSCredential)null;
            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri("http://" + hostName + "/Powershell"), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", psCredential);
            connectionInfo.OperationTimeout = 4 * 60 * 1000; // 4 minutes.
            connectionInfo.OpenTimeout = 1 * 60 * 1000; // 1 minute.


            using (Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo))
            {

                runspace.Open();
                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    // Install-TransportAgent -Name "Exchange DkimSigner" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "$EXDIR\ExchangeDkimSigner.dll"
                    powershell.AddCommand("Install-TransportAgent");
                    powershell.AddParameter("Name", "Exchange DkimSigner");
                    powershell.AddParameter("TransportAgentFactory", "Exchange.DkimSigner.DkimSigningRoutingAgentFactory");
                    powershell.AddParameter("AssemblyPath", System.IO.Path.Combine(baseDir, "ExchangeDkimSigner.dll"));

                    // Enable-TransportAgent -Identity "Exchange DkimSigner"
                    powershell.AddCommand("Enable-TransportAgent");
                    powershell.AddParameter("Identity", "Exchange DkimSigner");


                    powershell.AddCommand("Get-TransportAgent");


                    Collection<PSObject> results = powershell.Invoke();
                    if (powershell.Streams.Error.Count > 0)
                    {
                        foreach (ErrorRecord error in powershell.Streams.Error)
                        {
                            MessageBox.Show(error.ToString());
                        }
                    }

                    foreach (PSObject result in results)
                    {
                        System.Windows.Forms.MessageBox.Show(
                             string.Format("Name: {0}",
                                 result.Properties["Identity"].Value.ToString()
                                 ));
                    }

                    int newPriority = 9;


                    powershell.Commands.Clear();

                    //Set-TransportAgent -Identity "Exchange DkimSigner" -Priority 3
                    powershell.AddCommand("Set-TransportAgent");
                    powershell.AddParameter("Identity", "Exchange DkimSigner");
                    powershell.AddParameter("Priority", newPriority);
                }
            }



            /*string userName = "Administrator";

            string password = "PWD";

            System.Security.SecureString securePassword = new System.Security.SecureString();

            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            PSCredential credential = new PSCredential(userName, securePassword);

            var wsConnectionInfo = new WSManConnectionInfo(new Uri("http://localhost/powershell"),
                                        "http://schemas.microsoft.com/powershell/Microsoft.Exchange", credential);

            wsConnectionInfo.AuthenticationMechanism = AuthenticationMechanism.Basic;

            Runspace myRunSpace = RunspaceFactory.CreateRunspace(wsConnectionInfo);*/


            /*RunspaceConfiguration rsConfig = RunspaceConfiguration.Create();

            PSSnapInException snapInException = null;
            // Ex 2007 Microsoft.Exchange.Management.PowerShell.Admin
            PSSnapInInfo info = rsConfig.AddPSSnapIn("Microsoft.Exchange.Management.PowerShell.E2010", out snapInException);
            PSSnapInInfo info2 = rsConfig.AddPSSnapIn("Microsoft.Exchange.Management.PowerShell.Setup", out snapInException);
            PSSnapInInfo info3 = rsConfig.AddPSSnapIn("Microsoft.Exchange.Management.PowerShell.Support", out snapInException);*/


            /*string hostName = System.Net.Dns.GetHostEntry("").HostName;

            PSCredential psCredential = (PSCredential)null;
            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri("http://" + hostName + "/Powershell"), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", psCredential);
            connectionInfo.OperationTimeout = 4 * 60 * 1000; // 4 minutes.
            connectionInfo.OpenTimeout = 1 * 60 * 1000; // 1 minute.


            using (Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo))
            {

                runspace.Open();
                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;
                    //Create the command and add a parameter
                    powershell.AddCommand("Get-TransportAgent");
                    powershell.AddParameter("RecipientTypeDetails", "UserMailbox");
                    //Invoke the command and store the results in a PSObject collection
                    Collection<PSObject> results = powershell.Invoke();
                    //Iterate through the results and write the DisplayName and PrimarySMTP
                    //address for each mailbox
                    foreach (PSObject result in results)
                   {
                       System.Windows.Forms.MessageBox.Show(
                            string.Format("Name: {0}",
                                result.Properties["Identity"].Value.ToString()
                                ));
                    }
                }
            }*/


            /*Runspace myRunSpace = RunspaceFactory.CreateRunspace(rsConfig);
            myRunSpace.Open();
            Pipeline pipeLine = myRunSpace.CreatePipeline();
            Command myCommand = new Command("Get-Mailbox");
            //CommandParameter psSnapInParam = new CommandParameter("PSSnapIn", "Microsoft.Exchange.Management.PowerShell.Admin");
            //myCommand.Parameters.Add(psSnapInParam);
            //CommandParameter verbParam = new CommandParameter("Verb", "Get");
            pipeLine.Commands.Add(myCommand);
            Collection<PSObject> commandResults = pipeLine.Invoke();
            foreach (PSObject cmdlet in commandResults)
            {   
                string cmdletName = cmdlet.Properties["Name"].Value.ToString();
                System.Windows.Forms.MessageBox.Show(cmdletName +" " + cmdlet.ToString());
            }*/







            return true;
        }
    }

}
