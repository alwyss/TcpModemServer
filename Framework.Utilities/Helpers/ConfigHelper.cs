using System;
using System.Configuration;
using Framework.Utilities.Tracing;
using ProviderBacnet;

namespace Framework.Utilities.Helpers
{
    public class ConfigHelper
    {
        public static T GetConfig<T>(string key, T defaultValue = default(T))
        {
            try
            {
                var config = GetConfig(key);
                T result;
                if (config == null)
                {
                    result = defaultValue;
                }
                else
                {
                    if (!ParserHelper.TryParse(config, out result))
                    {
                        result = defaultValue;
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                TraceUtil.Error("Invalid config: {0}", key, exception);
                return defaultValue;
            }
        }

        public static string GetConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void SetConfig(string key, string value)
        {
            ConfigurationManager.AppSettings[key] = value;
        }
    }
}
