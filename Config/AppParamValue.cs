using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackLib
{
    public static class AppParamValue
    {
        private readonly static object _configLock = new object();

        #region AppSettings
        private static TType GetAppSettings<TType>(string key, TType defaultValue = default)
        {
            AppSettingsReader appSettings = new AppSettingsReader();
            return (TType)appSettings.GetValue(key, typeof(TType));
        }

        private static void SetAppSettings<TType>(string key, TType value)
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = appConfig.AppSettings;
            if (appSettings.Settings[key] == null)
                appSettings.Settings.Add(key, value.ToString());
            else
                appSettings.Settings[key].Value = value.ToString();
            appConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion AppSettings

        #region Section
        private static ConfigurationSection GetSection(string sectionName)
        {
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return appConfig.GetSection(sectionName);
        }

        private static void SetSection(string sectionName, ConfigurationSection value)
        {
            value.CurrentConfiguration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(sectionName);
        }

        #endregion Section 
    }
}
