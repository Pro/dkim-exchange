using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

using ConfigurationSettings;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Configuration.DkimSigner
{
    public class ExchangeHelper
    {
        public const string AGENT_NAME = "Exchange DkimSigner";
        public static string AGENT_DIR = @"C:\Program Files\Exchange DkimSigner";

        /// <summary>
        /// Get the current Exchange version for the current server from Active Directy (ldap).
        /// 
        /// The format of the string is 'Version 14.1 (Build 30218.15)'
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

        public static Version getExchangeVersion()
        {
            string verStr = checkExchangeVersionInstalled();

            Match match = Regex.Match(verStr, @"Version (\d+)\.(\d+)\s\(Build\s(\d+)\.(\d+)\)", RegexOptions.IgnoreCase);


            if (!match.Success)
                return null;

            // compose full version number, cut off first two numbers from build part
            string fullVersion = match.Groups[1].ToString() + "." + match.Groups[2].ToString() + "." + match.Groups[3].ToString().Substring(2) + "." + match.Groups[4].ToString();

            return new Version(fullVersion);
        }

        /// <summary>
        /// Create the connection info needed to execute power shell commands using exchange management cmdlets
        /// </summary>
        /// <returns>The connection info</returns>
        private static WSManConnectionInfo getPSConnectionInfo()
        {
            string hostName = System.Net.Dns.GetHostEntry("").HostName;

            PSCredential psCredential = (PSCredential)null;
            WSManConnectionInfo connectionInfo = new WSManConnectionInfo(new Uri("http://" + hostName + "/Powershell"), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", psCredential);
            connectionInfo.OperationTimeout = 4 * 60 * 1000; // 4 minutes.
            connectionInfo.OpenTimeout = 1 * 60 * 1000; // 1 minute.
            return connectionInfo;
        }

        /// <summary>
        /// Checks if the last powerShell command failed with errors.
        /// If yes, this method will throw an ExchangeHelperException to notify the callee.
        /// </summary>
        /// <param name="powerShell">PowerShell to check for errors</param>
        /// <param name="errorPrependMessage">String prepended to the exception message</param>
        private static Collection<PSObject> invokePS(PowerShell powerShell, string errorPrependMessage)
        {
            Collection<PSObject> results = null;
            try
            {
                results = powerShell.Invoke();
            }
            catch (System.Management.Automation.RemoteException e)
            {
                if (errorPrependMessage.Length > 0)
                    throw new ExchangeHelperException("Error getting list of Transport Agents:\n" + e.Message, e);
                else
                    throw e;
            }
            if (powerShell.Streams.Error.Count > 0)
            {
                string errors = errorPrependMessage;
                if (errorPrependMessage.Length > 0 && !errorPrependMessage.EndsWith(":"))
                    errors += ":";

                foreach (ErrorRecord error in powerShell.Streams.Error)
                {
                    if (errors.Length > 0)
                        errors += "\n";
                    errors += error.ToString();
                }
                throw new ExchangeHelperException(errors);
            }
            return results;
        }

        /// <summary>
        /// Checks if the Exchange DKIM TransportAgent is already installed or not. It doesn't check if the agent is enabled.
        /// </summary>
        /// <returns>true if it is installed.</returns>
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

                    Collection<PSObject> results = invokePS(powershell, "Error getting list of Transport Agents");
                    foreach (PSObject result in results)
                    {
                        if (result.Properties["Identity"].Value.ToString().Equals(AGENT_NAME))
                        {
                            return true;
                        }
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Restart the MSExchangeTransport service. Needs to be called after changes on transport agent.
        /// 
        /// Throws ExchangeHelperException on error.
        /// </summary>
        public static void restartTransportService()
        {
            stopTransportService();
            startTransportService();
        }

        /// <summary>
        /// Stop the MSExchangeTransport service.
        /// 
        /// Throws ExchangeHelperException on error.
        /// </summary>
        public static void stopTransportService()
        {
            int timeoutMS = 60 * 1000; //ms
            ServiceController service = new ServiceController("MSExchangeTransport");
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMS);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (Exception e)
            {
                throw new ExchangeHelperException("Couldn't stop 'MSExchangeTransport' service\n" + e.Message, e);
            }
        }

        /// <summary>
        /// Check if the MSExchangeTransport service is running.
        /// 
        /// Throws ExchangeHelperException on error.
        /// </summary>
        public static bool isTransportServiceRunning()
        {
            ServiceController service = new ServiceController("MSExchangeTransport");
            return service.Status == ServiceControllerStatus.Running;
        }

        /// <summary>
        /// Start the MSExchangeTransport service.
        /// 
        /// Throws ExchangeHelperException on error.
        /// </summary>
        public static void startTransportService()
        {
            int timeoutMS = 60 * 1000; //ms
            ServiceController service = new ServiceController("MSExchangeTransport");
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMS);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception e)
            {
                throw new ExchangeHelperException("Couldn't start 'MSExchangeTransport' service\n" + e.Message, e);
            }
        }

        /// <summary>
        /// Uninstalls the transport agent by calling the corresponding PowerShell commands (Disable-TransportAgent and Uninstall-TransportAgent).
        /// You need to restart the MSExchangeTransport service after uninstall.
        /// 
        /// Throws ExchangeHelperException on error.
        /// </summary>
        public static void uninstallTransportAgent()
        {
            if (!isAgentInstalled())
                return;

            using (Runspace runspace = RunspaceFactory.CreateRunspace(getPSConnectionInfo()))
            {
                runspace.Open();

                using (PowerShell powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;

                    // Disable-TransportAgent -Identity "Exchange DkimSigner" 
                    powershell.AddScript("Disable-TransportAgent -Confirm:$false -Identity \"" + AGENT_NAME + "\"");

                    // Uninstall-TransportAgent -Identity "Exchange DkimSigner"  
                    powershell.AddScript("Uninstall-TransportAgent -Confirm:$false -Identity \"" + AGENT_NAME + "\"");

                    Collection<PSObject> results = invokePS(powershell, "Error uninstalling Transport Agent");
                }
            }

        }

        /// <summary>
        /// Installs the transport agent by calling the corresponding PowerShell commands (Install-TransportAgent and Enable-TransportAgent).
        /// The priority of the agent is set to the highest one.
        /// You need to restart the MSExchangeTransport service after install.
        /// 
        /// Throws ExchangeHelperException on error.
        /// </summary>
        public static void installTransportAgent()
        {
            // First make sure the following Registry key exists
            // HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM
            if (RegistryHelper.Open(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") != null)
            {
                RegistryHelper.WriteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM");
            }


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
                        powershell.AddParameter("Name", AGENT_NAME);
                        powershell.AddParameter("TransportAgentFactory", "Exchange.DkimSigner.DkimSigningRoutingAgentFactory");
                        powershell.AddParameter("AssemblyPath", System.IO.Path.Combine(AGENT_DIR, "ExchangeDkimSigner.dll"));

                        results = invokePS(powershell, "Error installing Transport Agent");

                        if (results.Count == 1)
                        {
                            currPriority = Int32.Parse(results[0].Properties["Priority"].Value.ToString());
                        }

                        powershell.Commands.Clear();
                        
                        // Enable-TransportAgent -Identity "Exchange DkimSigner"
                        powershell.AddCommand("Enable-TransportAgent");
                        powershell.AddParameter("Identity", AGENT_NAME);

                        invokePS(powershell, "Error enabling Transport Agent");
                    }

                    powershell.Commands.Clear();
                    
                    // Determine current maximum priority
                    powershell.AddCommand("Get-TransportAgent");

                    results = invokePS(powershell, "Error getting list of Transport Agents");
                    
                    int maxPrio = 0;
                    foreach (PSObject result in results)
                    {
                        
                        if (!result.Properties["Identity"].Value.ToString().Equals(AGENT_NAME)){
                            maxPrio = Math.Max(maxPrio, Int32.Parse(result.Properties["Priority"].Value.ToString()));
                        }
                    }
                    
                    powershell.Commands.Clear();

                    if (currPriority != maxPrio + 1)
                    {
                        //Set-TransportAgent -Identity "Exchange DkimSigner" -Priority 3
                        powershell.AddCommand("Set-TransportAgent");
                        powershell.AddParameter("Identity", AGENT_NAME);
                        powershell.AddParameter("Priority", maxPrio + 1);
                        results = invokePS(powershell, "Error setting priority of Transport Agent");
                    }
                }
            }

        }
    }

    [Serializable]
    public class ExchangeHelperException : Exception
    {

        public ExchangeHelperException(string message)
            : base(message) { }

        public ExchangeHelperException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public ExchangeHelperException(string message, Exception innerException)
            : base(message, innerException) { }

        public ExchangeHelperException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected ExchangeHelperException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}