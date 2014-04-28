using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationSettings
{
    class RegistryHelper
    {
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
            RegistryKey sk1 = rk.OpenSubKey(subKey);

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
        public static string Read(string keyName, string subKey = "")
        {
            string value = null;
            RegistryKey sk1 = Open(subKey);

            if (sk1 != null)
            {
                try
                {
                    value = (string)sk1.GetValue(keyName);
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
        public static bool Write(string keyName, object Value, string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(subKey);
                sk1.SetValue(keyName, Value, RegistryValueKind.String);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Create a sub key
        /// </summary>
        /// <param name="subKey">Subkey to create</param>
        /// <returns></returns>
        public static bool WriteSubKeyTree(string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(subKey);

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
        public static bool DeleteKey(string keyName, string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(subKey);

                if(sk1 != null)
                    sk1.DeleteValue(keyName);

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
        public static bool DeleteSubKeyTree(string keyName, string subKey = "")
        {
            try
            {
                RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                            Environment.Is64BitOperatingSystem
                                                            ? RegistryView.Registry64
                                                            : RegistryView.Registry32);
                RegistryKey sk1 = rk.CreateSubKey(subKey);

                if (sk1 != null)
                    sk1.DeleteSubKeyTree(keyName);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
