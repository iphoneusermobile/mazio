using System;
using mazio.util;
using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace mazio {
    internal class SettingsWrapper {
        public static T getSettingValue<T>(string key, T defaultValue) {
            return isPortable() ? getIniSettingValue(key, defaultValue) : getRegistrySettingValue(key, defaultValue);
        }

        public static void saveSettingValue<T>(string key, T value) {
            if (isPortable()) {
                saveIniSettingValue(key, value);
            } else {
                saveRegistrySettingValue(key, value);
            }
        }

        private static bool isPortable() {
            try {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Kalamon\Mazio")) {
                    if (key == null) {
                        return true;
                    }
                    string value = (string) key.GetValue("Install_Dir");
                    if (value == null) {
                        return true;
                    }
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    if (assembly == null || assembly.Location == null) {
                        return true;
                    }
                    return !assembly.Location.Equals(value + "\\mazio.exe");
                }
            } catch (Exception) {
                return true;
            }
        }

        private static T getRegistrySettingValue<T>(string key, T defaultValue) {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (reg == null) {
                return defaultValue;
            }
            object value = reg.GetValue(key);
            if (value == null || !(value is T)) {
                return defaultValue;
            }
            return (T) value;
        }

        private static void saveRegistrySettingValue<T>(string key, T value) {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey(Constants.REG_KEY);
            if (reg == null) {
                return;
            }
            reg.SetValue(key, value);
        }

        private static string getIniPath() {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(path, Constants.INI_NAME);
        }

        private static T getIniSettingValue<T>(string key, T defaultValue) {
            IniFile ini = new IniFile(getIniPath());
            string iniValue = ini.IniReadValue(Constants.INI_SECTION, key);

            object value = null;
            if (typeof (T) == typeof (string)) {
                value = iniValue;
            } else if (typeof (T) == typeof (int)) {
                try {
                    value = Int32.Parse(iniValue);
                } catch (Exception) {
                    return defaultValue;
                }
            } else if (typeof (T) == typeof (double)) {
                try {
                    value = Double.Parse(iniValue);
                } catch (Exception) {
                    return defaultValue;
                }
            } else if (typeof (T) == typeof (bool)) {
                try {
                    value = Boolean.Parse(iniValue);
                } catch (Exception) {
                    return defaultValue;
                }
            }


            if (value == null || !(value is T)) {
                return defaultValue;
            }
            return (T) value;
        }

        private static void saveIniSettingValue<T>(string key, T value) {
            IniFile ini = new IniFile(getIniPath());
            ini.IniWriteValue(Constants.INI_SECTION, key, value.ToString());
        }
    }
}