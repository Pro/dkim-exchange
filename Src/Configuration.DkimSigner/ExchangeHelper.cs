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
using System.ServiceProcess;

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

                return version != string.Empty ? version : "Not installed";
            }
            catch (Exception)
            {
                return "Not installed";
            }
        }

        private static WSManConnectionInfo getPSConnectionInfo()
        {

            string hostName = System.Net.Dns.GetHostEntry("").HostName;

            PSCredential psCredential = (PSCredential)null;
            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri("http://" + hostName + "/Powershell"), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", psCredential);
            connectionInfo.OperationTimeout = 4 * 60 * 1000; // 4 minutes.
            connectionInfo.OpenTimeout = 1 * 60 * 1000; // 1 minute.
            return connectionInfo;
        }

        public static bool isAgentInstalled()
        {
            using (Runspace runspace = RunspaceFactory.CreateRunspace(getPSConnectionInfo()))
            {

                runspace.Open();
                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    // First check if Transport Agent exists already
                    powershell.AddCommand("Get-TransportAgent");

                    Collection<PSObject> results = powershell.Invoke();
                    if (powershell.Streams.Error.Count > 0)
                    {
                        foreach (ErrorRecord error in powershell.Streams.Error)
                        {

                            MessageBox.Show("Error getting list of Transport Agents\n" + error.ToString(), "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    foreach (PSObject result in results)
                    {
                        if (result.Properties["Identity"].Value.ToString().Equals("Exchange DkimSigner"))
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public static bool restartTransportService()
        {
            int timeoutMS = 60 * 1000; //ms
            ServiceController service = new ServiceController("MSExchangeTransport");
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMS);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMS - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't restart 'MSExchangeTransport' service\n" + e.Message, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static bool uninstallTransportAgent()
        {

            if (!isAgentInstalled())
                return true;

            using (Runspace runspace = RunspaceFactory.CreateRunspace(getPSConnectionInfo()))
            {

                runspace.Open();
                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    // Disable-TransportAgent -Identity "Exchange DkimSigner" 
                    powershell.AddScript("Disable-TransportAgent -Confirm:$false -Identity \"Exchange DkimSigner\"");
                    // Uninstall-TransportAgent -Identity "Exchange DkimSigner"  
                    powershell.AddScript("Uninstall-TransportAgent -Confirm:$false -Identity \"Exchange DkimSigner\"");

                    Collection<PSObject> results = null;
                    try
                    {
                        results = powershell.Invoke();
                    }
                    catch (System.Management.Automation.RemoteException e)
                    {
                        MessageBox.Show("Error uninstalling Transport Agent\n" + e.Message, "PowerShell exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (powershell.Streams.Error.Count > 0)
                    {
                        foreach (ErrorRecord error in powershell.Streams.Error)
                        {
                            MessageBox.Show("Error uninstalling Transport Agent\n" + error.ToString(), "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                }
            }

            return restartTransportService();
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



            using (Runspace runspace = RunspaceFactory.CreateRunspace(getPSConnectionInfo()))
            {

                runspace.Open();
                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    int currPriority = 0;
                    Collection<PSObject> results;

                    if (!isAgentInstalled())
                    {
                        // Install-TransportAgent -Name "Exchange DkimSigner" -TransportAgentFactory "Exchange.DkimSigner.DkimSigningRoutingAgentFactory" -AssemblyPath "$EXDIR\ExchangeDkimSigner.dll"
                        powershell.AddCommand("Install-TransportAgent");
                        powershell.AddParameter("Name", "Exchange DkimSigner");
                        powershell.AddParameter("TransportAgentFactory", "Exchange.DkimSigner.DkimSigningRoutingAgentFactory");
                        powershell.AddParameter("AssemblyPath", System.IO.Path.Combine(baseDir, "ExchangeDkimSigner.dll"));

                        results = powershell.Invoke();
                        if (powershell.Streams.Error.Count > 0)
                        {
                            foreach (ErrorRecord error in powershell.Streams.Error)
                            {
                                MessageBox.Show("Error installing Transport Agent\n" + error.ToString(), "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return false;
                        }

                        if (results.Count == 1)
                        {
                            currPriority = Int32.Parse(results[0].Properties["Priority"].Value.ToString());
                        }

                        powershell.Commands.Clear();
                        // Enable-TransportAgent -Identity "Exchange DkimSigner"
                        powershell.AddCommand("Enable-TransportAgent");
                        powershell.AddParameter("Identity", "Exchange DkimSigner");



                        results = powershell.Invoke();
                        if (powershell.Streams.Error.Count > 0)
                        {
                            foreach (ErrorRecord error in powershell.Streams.Error)
                            {
                                MessageBox.Show("Error enabling Transport Agent\n" + error.ToString(), "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return false;
                        }
                    }



                    powershell.Commands.Clear();
                    // Determine current maximum priority
                    powershell.AddCommand("Get-TransportAgent");

                    results = powershell.Invoke();
                    if (powershell.Streams.Error.Count > 0)
                    {
                        foreach (ErrorRecord error in powershell.Streams.Error)
                        {
                            MessageBox.Show("Error getting list of Transport Agents\n" + error.ToString(), "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    int maxPrio = 0;
                    foreach (PSObject result in results)
                    {
                        
                        if (!result.Properties["Identity"].Value.ToString().Equals("Exchange DkimSigner")){
                            maxPrio = Math.Max(maxPrio, Int32.Parse(result.Properties["Priority"].Value.ToString()));
                        }
                    }
                    
                    powershell.Commands.Clear();

                    if (currPriority != maxPrio + 1)
                    {

                        //Set-TransportAgent -Identity "Exchange DkimSigner" -Priority 3
                        powershell.AddCommand("Set-TransportAgent");
                        powershell.AddParameter("Identity", "Exchange DkimSigner");
                        powershell.AddParameter("Priority", maxPrio + 1);
                        results = powershell.Invoke();
                        if (powershell.Streams.Error.Count > 0)
                        {
                            foreach (ErrorRecord error in powershell.Streams.Error)
                            {
                                MessageBox.Show("Error setting priority of Transport Agent\n" + error.ToString(), "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return false;
                        }
                    }
                }
            }


            return restartTransportService();
        }
    }

}
