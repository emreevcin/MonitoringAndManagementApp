using Serilog;
using System;
using System.Collections.Generic;

namespace Util
{
    public static class SettingsJsonHelper
    {
        private static readonly ILogger logger = SerilogHelper.GetLogger();
        private static readonly ISettingsRepository settingsRepository = new JsonSettingsRepository(Constants.settingsFilePath);

        public static void SaveServiceSettings(string serviceKey, ServiceSettingsDto serviceSettings)
        {
            try
            {
                var allSettings = settingsRepository.LoadAllSettings() ?? new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();
                UpdateServiceSettings(allSettings, serviceKey, serviceSettings);
                settingsRepository.SaveAllSettings(allSettings);
            }
            catch (Exception ex)
            {
                logger.Error($"Error saving service settings: {ex.Message}");
                throw new Exception("Error saving service settings", ex);
            }
        }

        public static ServiceSettingsDto LoadServiceSettings(string serviceKey)
        {
            try
            {
                var allSettings = settingsRepository.LoadAllSettings();
                if (allSettings != null)
                {
                    string categoryName = GetCategoryName(serviceKey);
                    if (allSettings.ContainsKey(categoryName) && allSettings[categoryName].ContainsKey(serviceKey))
                    {
                        return allSettings[categoryName][serviceKey];
                    }
                    else
                    {
                        logger.Warning($"Settings for {serviceKey} not found in category {categoryName}. Returning default settings.");
                        return CreateDefaultServiceSettings(serviceKey);
                    }
                }
                logger.Warning("All settings are null. Returning default settings.");
                return CreateDefaultServiceSettings(serviceKey);
            }
            catch (Exception ex)
            {
                logger.Error($"Error loading settings: {ex.Message}");
                throw new Exception("Error loading settings:", ex);
            }
        }

        public static Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadAllServiceSettings()
        {
            try
            {
                var allSettings = settingsRepository.LoadAllSettings();
                if (allSettings == null)
                {
                    logger.Warning("All settings are null. No settings loaded.");
                    allSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();
                }
                return allSettings;
            }
            catch (Exception ex)
            {
                logger.Error($"Error loading all service settings: {ex.Message}");
                throw new Exception("Error loading all service settings:", ex);
            }
        }

        private static void UpdateServiceSettings(Dictionary<string, Dictionary<string, ServiceSettingsDto>> allSettings, string serviceKey, ServiceSettingsDto serviceSettings)
        {
            string categoryName = GetCategoryName(serviceKey);
            if (!allSettings.ContainsKey(categoryName))
            {
                allSettings[categoryName] = new Dictionary<string, ServiceSettingsDto>();
                logger.Information($"Added new category '{categoryName}' to settings.");
            }
            allSettings[categoryName][serviceKey] = serviceSettings;
        }

        private static string GetCategoryName(string serviceKey)
        {
            return serviceKey.Contains("Service") ? "Services" : "WebApis";
        }

        private static ServiceSettingsDto CreateDefaultServiceSettings(string serviceKey)
        {
            logger.Information($"Creating default settings for {serviceKey}");
            return new ServiceSettingsDto(serviceKey);
        }
    }
}
