using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace GMC
{
    public static class RegUtils
    {
        public static object? ReadValue(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
                return (subKey == null) ? null : subKey.GetValue(valueName);
        }

        public static string? ReadValueString(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
            {
                if (subKey == null)
                    return null;
                object? val = subKey.GetValue(valueName);
                if (val == null)
                    return null;
                if (subKey.GetValueKind(valueName) == RegistryValueKind.String)
                {
                    return (string)val;
                }
                else
                    throw new NotMatchingRegValueTypeException(RegistryValueKind.String, subKey.GetValueKind(valueName), hive, key, valueName, val);
            }
        }

        public static string[]? ListValueNames(RegistryHive hive, string key)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
            {
                if (subKey == null)
                    return null;
                return subKey.GetValueNames();

            }
        }

        public static RegistryValueKind? GetValueKind(RegistryHive hive, string key, string valueName)
        {
            if (!ValuePresent(hive, key, valueName))
            {
                return null;
            }

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey subKey = baseKey.OpenSubKey(key, writable: true) ?? baseKey.CreateSubKey(key, writable: true))
                return subKey.GetValueKind(valueName);
        }

        public static void WriteValue(RegistryHive hive, string key, string valueName, object value)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            {
                using (RegistryKey subKey = baseKey.OpenSubKey(key, writable: true) ?? baseKey.CreateSubKey(key, writable: true))
                {
                    subKey.SetValue(valueName, value);
                }
            }
        }

        public static void DeleteKeyTree(RegistryHive hive, string key)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
                baseKey.DeleteSubKeyTree(key, false);
        }

        public static void DeleteValue(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            {
                using (RegistryKey? subKey = baseKey.OpenSubKey(key, writable: true))
                {
                    if (subKey == null)
                        return;
                    if (valueName.Equals(""))
                        subKey.SetValue("", "");
                    else
                        subKey.DeleteValue(valueName, false);
                }
            }
        }

        public static bool ValuePresent(RegistryHive hive, string key, string valueName)
        {
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64))
            using (RegistryKey? subKey = baseKey.OpenSubKey(key))
                return (subKey == null) ? false : subKey.GetValue(valueName) != null;
        }

        public static RegistryHive? GetHive(string path)
        {
            if (!path.Contains("\\"))
                return null;
            string HiveRaw = path.Split("\\")[0];
            switch (HiveRaw)
            {
                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;
                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;
                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;
                case "HKEY_USERS":
                    return RegistryHive.Users;
                case "HKEY_CURRENT_CONFIG":
                    return RegistryHive.CurrentConfig;
            }

            return null;
        }

        public static string? GetKeyPath(string path)
        {
            if (!path.Contains("\\") || path.IndexOf('\\') + 1 == path.Length)
                return null;

            return path.Substring(path.IndexOf('\\') + 1);
        }
    }

    public class NotMatchingRegValueTypeException : Exception
    {
        RegistryHive hive;
        RegistryValueKind requestedType;
        RegistryValueKind gotType;
        string key;
        string valueName;
        object? value;
        public NotMatchingRegValueTypeException(RegistryValueKind _req, RegistryValueKind _got, RegistryHive _hive, string _key, string _valueName, object? _value)
        {
            value = _value;
            hive = _hive;
            requestedType = _req;
            gotType = _got;
            key = _key;
            valueName = _valueName;
        }
    }

}
