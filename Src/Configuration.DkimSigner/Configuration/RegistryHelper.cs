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

        /// <summary>
        /// Open a subkey and return it for manipulation
        /// </summary>
        /// <param name="subKey">Subkey to open</param>
        /// <returns></returns>
        public static RegistryKey Open(string subKey = "")
        {
            RegistryKey rk = RegistryKey.OpenBaseKey(   RegistryHive.LocalMachine,
                                                        Environment.Is64BitOperatingSystem
                                                        ? RegistryView.Registry64
                                                        : RegistryView.Registry32);
            RegistryKey sk1 = rk.OpenSubKey(BASE_REGISTRY_KEY + subKey);

            return (sk1 != null ? sk1 : null);
        }

        /// <summary>
        /// Get all subkey names in the specific subkey
        /// </summary>
        /// <param name="subKey">Specific subkey</param>
        /// <returns></returns>
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

        /// <summary>
        /// Read value from a key
        /// </summary>
        /// <param name="KeyName">Key name</param>
        /// <param name="subKey">Subkey that contain key name</param>
        /// <returns></returns>
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

        /// <summary>
        /// Write value to a key
        /// </summary>
        /// <param name="KeyName">Key name</param>
        /// <param name="Value">Value to write</param>
        /// <param name="subKey">Subkey that contain key name</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete a key that containt a value
        /// </summary>
        /// <param name="KeyName">Key to delete</param>
        /// <param name="subKey">Subkey that contain key name</param>
        /// <returns></returns>
        public bool DeleteKey(string KeyName, string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(BASE_REGISTRY_KEY + subKey);

                if(sk1 != null)
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete a complete subkey tree
        /// </summary>
        /// <param name="subKey">Subkey to delete</param>
        /// <returns></returns>
        public bool DeleteSubKeyTree(string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(BASE_REGISTRY_KEY + subKey);

                if (sk1 != null)
                    rk.DeleteSubKeyTree(subKey);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
