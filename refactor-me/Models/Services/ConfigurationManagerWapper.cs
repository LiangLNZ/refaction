using System;
using System.Configuration;

namespace refactor_me.Models.Services
{
    public class ConfigurationManagerWapper : IConfigurationManagerWapper
    {
        public string GetConnectionString(string name)
        {
             return GetValue(name).ConnectionString;
        }

        public string GetDbProviderName(string name)
        {
            return GetValue(name).ProviderName;
        }

        public string GetAppSettingValue(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                throw new Exception($"[{key}] is missing from the appSettings configuration section.");
            }
            return value;
        }

        private static ConnectionStringSettings GetValue(string name)
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStrings == null)
            {
                throw new Exception($"[{name}] is missing from the connectionStrings configuration section.");
            }
            return connectionStrings;
        }
    }
}