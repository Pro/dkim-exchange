using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Configuration.DkimSigner.Configuration
{
	public static class UninstallerRegistry
	{
		private static string registryKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\" + Constants.DkimSignerAgentName;

		/// <summary>
		/// This method creates an entry for the Programs and Files list to be able to uninstall the DKIM Signer from within this list.
		/// </summary>
		public static void Register()
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey, true);

			if (key == null)
				key = Registry.LocalMachine.CreateSubKey(registryKey);

			string version = Constants.GetShortenedVersionString(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
			DateTime today = DateTime.Today;

			key.SetValue("DisplayName", $"{Constants.DkimSignerAgentName} {version}");
			key.SetValue("DisplayIcon", Path.Combine(Constants.DkimSignerPath, Constants.DkimSignerConfigurationExe));
			key.SetValue("DisplayVersion", version);
			key.SetValue("Publisher", "dkim-exchange");
			key.SetValue("HelpLink", Constants.DkimSignerWebsite);
			key.SetValue("InstallDate", $"{today.Year}{today.Month}{today.Day}");
			key.SetValue("InstallLocation", Constants.DkimSignerPath);
			key.SetValue("URLInfoAbout", Constants.DkimSignerWebsite);
			key.SetValue("Comments", "DKIM Signer for Microsoft Exchange Server");
			key.SetValue("UninstallString", Path.Combine(Constants.DkimSignerPath, Constants.DkimSignerConfigurationExe) + " --uninstall");
		}

		public static void Unregister()
		{
			Registry.LocalMachine.DeleteSubKey(registryKey);
		}
	}
}