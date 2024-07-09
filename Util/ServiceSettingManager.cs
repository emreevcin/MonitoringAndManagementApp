using System;
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

        private static string GetCategoryName(string serviceName)
        {
            return serviceName.Contains("Service") ? "Services" : "WebApis";
        }

        private static string GetServiceSetting(string serviceName, string propertyName)
        {
            try
            {
                var allSettings = settingsRepository.LoadAllSettings();
                if (allSettings != null)
                {
                    string categoryName = GetCategoryName(serviceName);
                    if (allSettings.ContainsKey(categoryName) && allSettings[categoryName].ContainsKey(serviceName))
                    {
                        var serviceSettings = allSettings[categoryName][serviceName];
                        var property = serviceSettings.GetType().GetProperty(propertyName);
                        if (property != null)
                        {
                            return property.GetValue(serviceSettings)?.ToString();
                        }
                        else
                        {
                            logger.Warning($"Property '{propertyName}' not found for service '{serviceName}'.");
                        }
                    }
                    else
                    {
                        logger.Warning($"Service {serviceName} or property {propertyName} not found.");
                    }
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
