using System;

namespace LabXand.Core
{
    public class ConfigurationHelper
    {
        static string GetApplicationSettingValue(string key)
        {
            string value = System.Configuration.ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public static T GetApplicationSettingValue<T>(string key)
        {
            return GetApplicationSettingValue(key, default(T));
        }

        static T GetApplicationSettingValue<T>(string key , T defaultValue)
        {
            if (string.IsNullOrEmpty(GetApplicationSettingValue(key)))
                return defaultValue;
            try
            {
                object temp = Convert.ChangeType(GetApplicationSettingValue(key), typeof(T));
                return (T)temp;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not convert string to '{0}' type.", typeof(T).FullName), ex);
            }
        }
    }
}
