using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;

namespace Configuration.DkimSigner.Exchange
{
    public class ExchangeServer
    {
        /**********************************************************/
        /*********************** Constants ************************/
        /**********************************************************/

        private const string REGEX_INSTALLED_VERSION = "AdminDisplayVersion\\s:\\sVersion\\s(\\d+)\\.(\\d+)\\s\\(Build\\s(\\d+)\\.(\\d+)\\)";
        private const string REGEX_TRANSPORT_SERVICE_AGENT = "(?<name>.*?)\\s*(?<status>(True|False))\\s*(?<priority>\\d+?)";

        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/


        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public ExchangeServer() { }

        /**********************************************************/
        /*********************** Methods **************************/
        /**********************************************************/

        public string GetInstalledVersion()
        {
            string sReturn = "Not installed";

            string sResult = PowerShellHelper.ExecPowerShellCommand("Get-ExchangeServer -Identity " + Dns.GetHostName() + " | fl AdminDisplayVersion", true);

            if (sResult != null)
            {
                Match oMatch = Regex.Match(sResult, REGEX_INSTALLED_VERSION, RegexOptions.IgnoreCase);
                
                if(oMatch.Success)
                {
                    sReturn = oMatch.Groups[1].ToString() + "." + oMatch.Groups[2].ToString() + "." + oMatch.Groups[3].ToString() + "." + oMatch.Groups[4].ToString();
                }
            }

            return sReturn;
        }

        public bool IsTransportServiceInstalled()
        {
            return (ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "MSExchangeTransport") != null);
        }

        public ServiceControllerStatus GetTransportServiceStatus()
        {
            if (!this.IsTransportServiceInstalled())
            {
                throw new ExchangeServerException("No service 'MSExchangeTransport' available.");
            }

            return new ServiceController("MSExchangeTransport").Status;
        }

        public bool IsTransportServiceRunning()
        {
            return (this.GetTransportServiceStatus() == ServiceControllerStatus.Running);
        }

        public bool IsTransportServiceStopped()
        {
            return (this.GetTransportServiceStatus() == ServiceControllerStatus.Stopped);
        }

        public void StartTransportService()
        {
            if (!this.IsTransportServiceStopped())
            {
                throw new ExchangeServerException("Couldn't start 'MSExchangeTransport' service because it's already running.");
            }

            try
            {
                TimeSpan oTimeout = TimeSpan.FromMilliseconds(60000);
                ServiceController oService = new ServiceController("MSExchangeTransport");
                oService.Start();
                oService.WaitForStatus(ServiceControllerStatus.Running, oTimeout);
            }
            catch (Exception e)
            {
                throw new ExchangeServerException("Couldn't start 'MSExchangeTransport' service :\n" + e.Message, e);
            }
        }

        public void StopTransportService()
        {
            if (!this.IsTransportServiceRunning())
            {
                throw new ExchangeServerException("Couldn't stop 'MSExchangeTransport' service because it's already stopped.");
            }

            try
            {
                TimeSpan oTimeout = TimeSpan.FromMilliseconds(60000);
                ServiceController oService = new ServiceController("MSExchangeTransport");
                oService.Stop();
                oService.WaitForStatus(ServiceControllerStatus.Stopped, oTimeout);
            }
            catch (Exception e)
            {
                throw new ExchangeServerException("Couldn't stop 'MSExchangeTransport' service :\n" + e.Message, e);
            }
        }

        public void RestartTransportService()
        {
            this.StopTransportService();
            this.StartTransportService();
        }

        public List<TransportServiceAgent> GetTransportServiceAgents()
        {
            List<TransportServiceAgent> aoAgent = new List<TransportServiceAgent>();
            string sResult = PowerShellHelper.ExecPowerShellCommand("Get-TransportAgent", true);

            if (sResult != null)
            {
                string[] asLine = sResult.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string sLine in asLine)
                {
                    Match oMatch = Regex.Match(sLine, REGEX_TRANSPORT_SERVICE_AGENT);

                    if(oMatch.Success)
                    {
                        string sValue = oMatch.Groups["name"].Value;
                        bool bEnabled = string.Equals(oMatch.Groups["status"].Value, "True", StringComparison.OrdinalIgnoreCase);
                        int iPriority = int.Parse(oMatch.Groups["priority"].Value);
                        aoAgent.Add(new TransportServiceAgent(sValue, bEnabled, iPriority));
                    }
                }
            }

            return aoAgent;
        }

        public bool IsDkimAgentTransportInstalled()
        {
            bool bResult = false;

            foreach (TransportServiceAgent oAgent in this.GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DKIM_SIGNER_AGENT_NAME, StringComparison.Ordinal))
                {
                    bResult = true;
                    break;
                }
            }

            return bResult;
        }

        public bool IsDkimAgentTransportEnabled()
        {
            bool bResult = false;

            foreach (TransportServiceAgent oAgent in this.GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DKIM_SIGNER_AGENT_NAME, StringComparison.Ordinal))
                {
                    bResult = oAgent.Enabled;
                    break;
                }
            }

            return bResult;
        }

        public void InstallDkimTransportAgent()
        {
            if (!this.IsDkimAgentTransportInstalled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Install-TransportAgent -Confirm:$false -Name \"Exchange DkimSigner\" -TransportAgentFactory \"Exchange.DkimSigner.DkimSigningRoutingAgentFactory\" -AssemblyPath \"" + Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_AGENT_DLL) + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Install-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't install 'Exchange DkimSigner' agent :\n");
                }

                this.EnableDkimTransportAgent();
            }
        }

        public void UninstallDkimTransportAgent()
        {
            if (this.IsDkimAgentTransportInstalled())
            {
                this.DisableDkimTransportAgent();

                string sResult = PowerShellHelper.ExecPowerShellCommand("Uninstall-TransportAgent -Confirm:$false -Identity \"" + Constants.DKIM_SIGNER_AGENT_NAME + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Uninstall-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't install 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public void EnableDkimTransportAgent()
        {
            if (!this.IsDkimAgentTransportEnabled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Enable-TransportAgent -Confirm:$false -Identity \"" + Constants.DKIM_SIGNER_AGENT_NAME + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Enable-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't install 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public void DisableDkimTransportAgent()
        {
            if (this.IsDkimAgentTransportEnabled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Disable-TransportAgent -Confirm:$false -Confirm:$false -Identity \"" + Constants.DKIM_SIGNER_AGENT_NAME + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Disable-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't install 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public void SetPriorityDkimTransportAgent(int iPriority)
        {
            if (this.IsDkimAgentTransportEnabled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Set-TransportAgent -Confirm:$false -Identity \"" + Constants.DKIM_SIGNER_AGENT_NAME + "\" -Priority " + iPriority, true);

                Match oMatch = Regex.Match(sResult, "^Set-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't install 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public void MoveUpTransportAgent()
        {
            foreach (TransportServiceAgent oAgent in this.GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DKIM_SIGNER_AGENT_NAME, StringComparison.Ordinal))
                {
                    this.SetPriorityDkimTransportAgent(++oAgent.Priority);
                    break;
                }
            }
        }

        public void MoveDownTransportAgent()
        {
            foreach (TransportServiceAgent oAgent in this.GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DKIM_SIGNER_AGENT_NAME, StringComparison.Ordinal))
                {
                    this.SetPriorityDkimTransportAgent(--oAgent.Priority);
                    break;
                }
            }
        }
    }
}