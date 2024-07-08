using System;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Util
{
    public static class ServiceSettingManager
    {
        private static readonly ILogger logger = SerilogHelper.GetLogger();
        private static readonly ISettingsRepository settingsRepository = new JsonSettingsRepository(Constants.settingsFilePath);

        public static string GetServicePath(string serviceName)
        {
            return GetServiceSetting(serviceName, "FolderPath");
        }

        public static string GetServiceUrl(string serviceName)
        {
            return GetServiceSetting(serviceName, "Url");
        }

        private static string GetServiceSetting(string serviceName, string propertyName)
        {
            try
            {
                JObject allSettings = settingsRepository.LoadAllSettings();
                if (allSettings != null)
                {
                    if (allSettings.ContainsKey("Services") && allSettings["Services"][serviceName] != null)
                    {
                        var serviceSettings = allSettings["Services"][serviceName];
                        if (serviceSettings[propertyName] != null)
                        {
                            return serviceSettings[propertyName].ToString();
                        }
                        else
                        {
                            logger.Warning($"Property '{propertyName}' not found for service '{serviceName}'.");
                        }
                    }
                    logger.Warning($"Service {serviceName} or property {propertyName} not found.");
                }
                else
                {
                    logger.Warning("All settings are null.");
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error($"Error retrieving service {propertyName} for {serviceName}: {ex.Message}");
                throw new Exception($"Error retrieving service {propertyName} for {serviceName}: {ex.Message}");
            }
        }
    }
}
