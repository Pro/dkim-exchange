using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

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

            string version =
                Version.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion)
                    .ToString()
                    .Substring(0, 5);

            key.SetValue("DisplayName", Constants.DkimSignerAgentName + " " + version);
            key.SetValue("DisplayIcon", Path.Combine(Constants.DkimSignerPath, Constants.DkimSignerConfigurationExe));
            key.SetValue("DisplayVersion", version);
            key.SetValue("Publisher", "Stefan Profanter");
            key.SetValue("HelpLink", "https://github.com/Pro/dkim-exchange/issues");
            var now = DateTime.Now;
            key.SetValue("InstallDate", string.Format("{0}{1}{2}", now.Year, now.Month, now.Day));
            key.SetValue("InstallLocation", Constants.DkimSignerPath);
            key.SetValue("URLInfoAbout", "https://github.com/Pro/dkim-exchange");
            key.SetValue("Comments", "DKIM Signer for Microsoft Exchange Server");
            key.SetValue("UninstallString", Path.Combine(Constants.DkimSignerPath, Constants.DkimSignerConfigurationExe) + " --uninstall");
        }

        public static void Unregister()
        {
            Registry.LocalMachine.DeleteSubKey(registryKey);
        }
    }
}
