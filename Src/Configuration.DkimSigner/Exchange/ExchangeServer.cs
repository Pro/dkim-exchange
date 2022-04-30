using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Configuration.DkimSigner.Exchange
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

		public static bool IsSupportedVersion(Version exchangeVersion)
		{
			bool result = false;

			// Currently supported by this project:
			// Exchange 2013 CU23 or later (15.0.1497.2)
			// Exchange 2016 CU13 or later (15.1.1779.2)
			// Exchange 2019 RTM or later (15.2.221.12)
			// Versions of Exchange >= 15.3 should be OK due to stability of Microsoft.Exchange.Data.Common and Microsoft.Exchange.Data.Transport APIs
			// Versions of Exchange >= 16 are unsupported and require specific support to be added to Exchange.DkimSigner first

			Version minExchange2013 = new Version(15, 0, 1497, 2);
			Version minExchange2016 = new Version(15, 1, 1779, 2);
			Version minExchange2019 = new Version(15, 2, 221, 12);

			if (exchangeVersion.Major == 15)
			{
				if (exchangeVersion.Minor >= 3)
				{
					// Version of Exchange 15 later than Exchange 2019
					result = true;
				}
				else if ((exchangeVersion.Minor == 2) && (exchangeVersion.CompareTo(minExchange2019) >= 0))
				{
					// Exchange 2019 RTM or later
					result = true;
				}
				else if ((exchangeVersion.Minor == 1) && (exchangeVersion.CompareTo(minExchange2016) >= 0))
				{
					// Exchange 2016 CU13 or later
					result = true;
				}
				else if ((exchangeVersion.Minor == 0) && (exchangeVersion.CompareTo(minExchange2013) >= 0))
				{
					// Exchange 2013 CU23 or later
					result = true;
				}
			}

			return (result);
		}

		public static Version GetInstalledVersion()
		{
			Version result = new Version();
			string response;

			try
			{
				response = PowerShellHelper.ExecPowerShellCommand("Get-ExchangeServer -Identity " + Dns.GetHostName() + " | Format-List AdminDisplayVersion", true);
			}
			catch (Exception ex)
			{
				throw new ExchangeServerException(ex.Message);
			}

			if (response != null)
			{
				Match oMatch = Regex.Match(response, RegexInstalledVersion, RegexOptions.IgnoreCase);

				if (oMatch.Success)
				{
					result = new Version(int.Parse(oMatch.Groups[1].Value), int.Parse(oMatch.Groups[2].Value), int.Parse(oMatch.Groups[3].Value), int.Parse(oMatch.Groups[4].Value));
				}
			}

			return result;
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

					if (oMatch.Success)
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
	}
}