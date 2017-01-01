using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Exchange.DkimConfiguration.Exchange
{
    public class ExchangeServer
    {
        /**********************************************************/
        /*********************** Constants ************************/
        /**********************************************************/

        private const string RegexInstalledVersion = @"AdminDisplayVersion\s:\sVersion\s(\d+)\.(\d+)\s\(Build\s(\d+)\.(\d+)\)";
        private const string RegexTransportServiceAgent = @"(?<name>.*?)\s*(?<status>(True|False))\s*(?<priority>\d+)";

        /**********************************************************/
        /*********************** Methods **************************/
        /**********************************************************/

        public static string GetInstalledVersion()
        {
            string sReturn = "Not installed";

            string sResult;
            try
            {
                sResult = PowerShellHelper.ExecPowerShellCommand("Get-ExchangeServer -Identity " + Dns.GetHostName() + " | fl AdminDisplayVersion", true);
            }
            catch (Exception ex)
            {
                throw new ExchangeServerException(ex.Message);
            }

            if (sResult != null)
            {
                Match oMatch = Regex.Match(sResult, RegexInstalledVersion, RegexOptions.IgnoreCase);
                
                if(oMatch.Success)
                {
                    sReturn = oMatch.Groups[1] + "." + oMatch.Groups[2] + "." + oMatch.Groups[3] + "." + oMatch.Groups[4];
                }
            }

            return sReturn;
        }

        public static List<TransportServiceAgent> GetTransportServiceAgents()
        {
            List<TransportServiceAgent> aoAgent = new List<TransportServiceAgent>();
            string sResult = PowerShellHelper.ExecPowerShellCommand("Get-TransportAgent", true);

            if (sResult != null)
            {
                string[] asLine = sResult.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string sLine in asLine)
                {
                    Match oMatch = Regex.Match(sLine, RegexTransportServiceAgent);

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

        public static bool IsDkimAgentTransportInstalled()
        {
            bool bResult = false;

            foreach (TransportServiceAgent oAgent in GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DkimSignerAgentName, StringComparison.Ordinal))
                {
                    bResult = true;
                    break;
                }
            }

            return bResult;
        }

        public static bool IsDkimAgentTransportEnabled()
        {
            bool bResult = false;

            foreach (TransportServiceAgent oAgent in GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DkimSignerAgentName, StringComparison.Ordinal))
                {
                    bResult = oAgent.Enabled;
                    break;
                }
            }

            return bResult;
        }

        public static void InstallDkimTransportAgent()
        {
            if (!IsDkimAgentTransportInstalled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Install-TransportAgent -Confirm:$false -Name \"Exchange DkimSigner\" -TransportAgentFactory \"Exchange.DkimSigner.DkimSigningRoutingAgentFactory\" -AssemblyPath \"" + Path.Combine(Constants.DkimSignerPath, Constants.DkimSignerAgentDll) + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Install-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't install 'Exchange DkimSigner' agent :\n");
                }

                EnableDkimTransportAgent();
            }
        }

        public static void UninstallDkimTransportAgent()
        {
            if (IsDkimAgentTransportInstalled())
            {
                DisableDkimTransportAgent();

                string sResult = PowerShellHelper.ExecPowerShellCommand("Uninstall-TransportAgent -Confirm:$false -Identity \"" + Constants.DkimSignerAgentName + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Uninstall-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't uninstall 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public static void EnableDkimTransportAgent()
        {
            if (!IsDkimAgentTransportEnabled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Enable-TransportAgent -Confirm:$false -Identity \"" + Constants.DkimSignerAgentName + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Enable-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't enable 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public static void DisableDkimTransportAgent()
        {
            if (IsDkimAgentTransportEnabled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Disable-TransportAgent -Confirm:$false -Identity \"" + Constants.DkimSignerAgentName + "\"", true);

                Match oMatch = Regex.Match(sResult, "^Disable-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't disable 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public static void SetPriorityDkimTransportAgent(int iPriority)
        {
            if (IsDkimAgentTransportEnabled())
            {
                string sResult = PowerShellHelper.ExecPowerShellCommand("Set-TransportAgent -Confirm:$false -Identity \"" + Constants.DkimSignerAgentName + "\" -Priority " + iPriority, true);

                Match oMatch = Regex.Match(sResult, "^Set-TransportAgent", RegexOptions.IgnoreCase);
                if (oMatch.Success)
                {
                    throw new ExchangeServerException("Couldn't set priority of 'Exchange DkimSigner' agent :\n");
                }
            }
        }

        public static void MoveUpTransportAgent()
        {
            foreach (TransportServiceAgent oAgent in GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DkimSignerAgentName, StringComparison.Ordinal))
                {
                    SetPriorityDkimTransportAgent(++oAgent.Priority);
                    break;
                }
            }
        }

        public static void MoveDownTransportAgent()
        {
            foreach (TransportServiceAgent oAgent in GetTransportServiceAgents())
            {
                if (string.Equals(oAgent.Name, Constants.DkimSignerAgentName, StringComparison.Ordinal))
                {
                    SetPriorityDkimTransportAgent(--oAgent.Priority);
                    break;
                }
            }
        }
    }
}