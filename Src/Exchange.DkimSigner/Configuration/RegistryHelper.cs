using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConfigurationSettings
{
    /// <summary>
    /// Implement support for registry access from .NET 3.5, .NET 4 and higher
    /// http://dotnetgalactics.wordpress.com/2010/05/10/accessing-64-bit-registry-from-a-32-bit-process/
    /// </summary>
    class RegistryHelper
    {
        private const string BASE_REGISTRY_KEY = @"Software\";

        enum RegWow64Options
        {
            None = 0,
            KEY_WOW64_64KEY = 0x0100,
            KEY_WOW64_32KEY = 0x0200
        }
 
        enum RegistryRights
        {
            ReadKey = 131097,
            WriteKey = 131078
        }
 
        /// <summary>
        /// Open a registry key using the Wow64 node instead of the default 32-bit node.
        /// </summary>
        /// <param name="parentKey">Parent key to the key to be opened.</param>
        /// <param name="subKeyName">Name of the key to be opened</param>
        /// <param name="writable">Whether or not this key is writable</param>
        /// <param name="options">32-bit node or 64-bit node</param>
        /// <returns></returns>
        private static RegistryKey _openSubKey(RegistryKey parentKey, string subKeyName, bool writable, RegWow64Options options)
        {
            //Sanity check
            if (parentKey == null || _getRegistryKeyHandle(parentKey) == IntPtr.Zero)
            {
                return null;
            }
 
            //Set rights
            int rights = (int)RegistryRights.ReadKey;
            if (writable)
                rights = (int)RegistryRights.WriteKey;
 
            //Call the native function >.<
            int subKeyHandle, result = RegOpenKeyEx(_getRegistryKeyHandle(parentKey), subKeyName, 0, rights | (int)options, out subKeyHandle);
 
            //If we errored, return null
            if (result != 0)
            {
                return null;
            }
 
            //Get the key represented by the pointer returned by RegOpenKeyEx
            RegistryKey subKey = _pointerToRegistryKey((IntPtr)subKeyHandle, writable, false);
            return subKey;
        }
 
        /// <summary>
        /// Get a pointer to a registry key.
        /// </summary>
        /// <param name="registryKey">Registry key to obtain the pointer of.</param>
        /// <returns>Pointer to the given registry key.</returns>
        private static IntPtr _getRegistryKeyHandle(RegistryKey registryKey)
        {
            //Get the type of the RegistryKey
            Type registryKeyType = typeof(RegistryKey);
            //Get the FieldInfo of the 'hkey' member of RegistryKey
            System.Reflection.FieldInfo fieldInfo =
            registryKeyType.GetField("hkey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
 
            //Get the handle held by hkey
            SafeHandle handle = (SafeHandle)fieldInfo.GetValue(registryKey);
            //Get the unsafe handle
            IntPtr dangerousHandle = handle.DangerousGetHandle();
            return dangerousHandle;
        }
 
        /// <summary>
        /// Get a registry key from a pointer.
        /// </summary>
        /// <param name="hKey">Pointer to the registry key</param>
        /// <param name="writable">Whether or not the key is writable.</param>
        /// <param name="ownsHandle">Whether or not we own the handle.</param>
        /// <returns>Registry key pointed to by the given pointer.</returns>
        private static RegistryKey _pointerToRegistryKey(IntPtr hKey, bool writable, bool ownsHandle)
        {
            //Get the BindingFlags for private contructors
            System.Reflection.BindingFlags privateConstructors = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            //Get the Type for the SafeRegistryHandle
            Type safeRegistryHandleType = typeof(Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
            //Get the array of types matching the args of the ctor we want
            Type[] safeRegistryHandleCtorTypes = new Type[] { typeof(IntPtr), typeof(bool) };
            //Get the constructorinfo for our object
            System.Reflection.ConstructorInfo safeRegistryHandleCtorInfo = safeRegistryHandleType.GetConstructor(
            privateConstructors, null, safeRegistryHandleCtorTypes, null);
            //Invoke the constructor, getting us a SafeRegistryHandle
            Object safeHandle = safeRegistryHandleCtorInfo.Invoke(new Object[] { hKey, ownsHandle });
 
            //Get the type of a RegistryKey
            Type registryKeyType = typeof(RegistryKey);
            //Get the array of types matching the args of the ctor we want
            Type[] registryKeyConstructorTypes = new Type[] { safeRegistryHandleType, typeof(bool) };
            //Get the constructorinfo for our object
            System.Reflection.ConstructorInfo registryKeyCtorInfo = registryKeyType.GetConstructor(
            privateConstructors, null, registryKeyConstructorTypes, null);
            //Invoke the constructor, getting us a RegistryKey
            RegistryKey resultKey = (RegistryKey)registryKeyCtorInfo.Invoke(new Object[] { safeHandle, writable });
            //return the resulting key
            return resultKey;
        }
 
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int RegOpenKeyEx(IntPtr hKey, string subKey, int ulOptions, int samDesired, out int phkResult);

        public static RegistryKey Open(string subKey = "")
        {
            return _openSubKey(Registry.LocalMachine, BASE_REGISTRY_KEY + subKey, false, RegWow64Options.KEY_WOW64_64KEY);
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
                RegistryKey rk = _openSubKey(Registry.LocalMachine, @"SOFTWARE\" + BASE_REGISTRY_KEY + subKey, false, RegWow64Options.KEY_WOW64_64KEY);

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