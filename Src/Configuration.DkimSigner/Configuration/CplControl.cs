using Microsoft.Win32;
using System;
using System.Reflection;

namespace Configuration.DkimSigner.Configuration
{
	// https://msdn.microsoft.com/en-us/library/hh127450(v=vs.85).aspx

	public static class CplControl
	{
		private static Guid AssemblyGuid
		{
			get
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				return assembly.GetType().GUID;
			}
		}

		private static string LocalMachineKey
		{
			get
			{
				var keyPath = string.Format("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\ControlPanel\\NameSpace\\{0}{1}{2}", "{", AssemblyGuid, "}");
				return keyPath;
			}
		}

		private static string ClassesRootKey
		{
			get
			{
				var keyPath = string.Format("CLSID\\{0}{1}{2}", "{", AssemblyGuid, "}");
				return keyPath;
			}
		}

		public static void Register()
		{
			Unregister();

			var key = Registry.LocalMachine.CreateSubKey(LocalMachineKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
			key.SetValue(null, "Exchange DKIM Configuration", RegistryValueKind.String);
			key.Close();

			key = Registry.ClassesRoot.CreateSubKey(ClassesRootKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
			key.SetValue(null, "Exchange DKIM Configuration", RegistryValueKind.String);
			key.SetValue("InfoTip", "Configuration utility of DKIM Signing Agent for Microsoft Exchange Server", RegistryValueKind.String);
			key.SetValue("System.ApplicationName", "DKIMSigner.Configuration", RegistryValueKind.String);
			key.SetValue("System.ControlPanel.Category", "3,8", RegistryValueKind.String);
			key.Close();

			var keyPath = string.Format("{0}\\DefaultIcon", ClassesRootKey);
			var exeFullPath = string.Format("{0}\\{1}", Constants.DkimSignerPath, Constants.DkimSignerConfigurationExe);

			key = Registry.ClassesRoot.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
			key.SetValue(null, string.Format("{0}, {1}", exeFullPath, -2), RegistryValueKind.String);
			key.Close();

			keyPath = string.Format("{0}\\Shell\\Open\\Command", ClassesRootKey);
			key = Registry.ClassesRoot.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
			key.SetValue(null, exeFullPath, RegistryValueKind.ExpandString);
			key.Close();
		}

		public static void Unregister()
		{
			if (Registry.LocalMachine.OpenSubKey(LocalMachineKey) != null)
				Registry.LocalMachine.DeleteSubKey(LocalMachineKey);
			if (Registry.ClassesRoot.OpenSubKey(ClassesRootKey) != null)
				Registry.ClassesRoot.DeleteSubKeyTree(ClassesRootKey);
		}
	}
}