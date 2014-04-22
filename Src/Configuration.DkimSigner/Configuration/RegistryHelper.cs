using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationSettings
{
    class RegistryHelper
    {
        private const string BASE_REGISTRY_KEY = @"Software\";

        public static RegistryKey Open(string subKey = "")
        {
            RegistryKey rk = RegistryKey.OpenBaseKey(   RegistryHive.LocalMachine,
                                                        Environment.Is64BitOperatingSystem
                                                        ? RegistryView.Registry64
                                                        : RegistryView.Registry32);
            RegistryKey sk1 = rk.OpenSubKey(BASE_REGISTRY_KEY + subKey);

            return (sk1 != null ? sk1 : null);
        }

        public static string[] GetSubKeyName(string subKey = "")
        {
            string[] value = null;
            RegistryKey sk1 = Open(subKey);

            if (sk1 != null)
            {
                try
                {
                    value = sk1.GetSubKeyNames();
                }
                catch (Exception) { }
            }

            return value;
        }

        public static string Read(string KeyName, string subKey = "")
        {
            string value = null;
            RegistryKey sk1 = Open(subKey);

            if (sk1 != null)
            {
                try
                {
                    value = (string)sk1.GetValue(KeyName);
                }
                catch (Exception) { }
            }

            return value;
        }

        public static bool Write(string KeyName, object Value, string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(BASE_REGISTRY_KEY + subKey);
                sk1.SetValue(KeyName, Value, RegistryValueKind.String);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}