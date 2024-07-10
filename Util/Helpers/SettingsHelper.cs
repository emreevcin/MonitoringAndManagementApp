using Serilog;
using System;
using System.Collections.Generic;

namespace Util
{
    public static class SettingsHelper
    {
        private static readonly ILogger _logger = SerilogHelper.GetLogger();
        private static readonly ISettingsRepository _settingsRepository = new JsonSettingsRepository(Constants.settingsFilePath);

        public static void SaveServiceSettings(string serviceKey, ServiceSettingsDto serviceSettings)
        {
            try
            {
                var allSettings = _settingsRepository.LoadAllSettings() ?? new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();
                UpdateServiceSettings(allSettings, serviceKey, serviceSettings);
                _settingsRepository.SaveAllSettings(allSettings);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error saving service settings: {ex.Message}");
                throw new Exception("Error saving service settings", ex);
            }
        }

        public static ServiceSettingsDto LoadServiceSettings(string serviceKey)
        {
            try
            {
                var allSettings = _settingsRepository.LoadAllSettings();
                if (allSettings != null)
                {
                    string categoryName = GetCategoryName(serviceKey);
                    if (allSettings.ContainsKey(categoryName) && allSettings[categoryName].ContainsKey(serviceKey))
                    {
                        return allSettings[categoryName][serviceKey];
                    }
                    else
                    {
                        _logger.Warning($"Settings for {serviceKey} not found in category {categoryName}. Returning default settings.");
                        return new ServiceSettingsDto(serviceKey);
                    }
                }
                _logger.Warning("All settings are null. Returning default settings.");
                return new ServiceSettingsDto(serviceKey);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error loading settings: {ex.Message}");
                throw new Exception("Error loading settings:", ex);
            }
        }

        public static Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadAllServiceSettings()
        {
            try
            {
                var allSettings = _settingsRepository.LoadAllSettings();
                if (allSettings == null)
                {
                    _logger.Warning("All settings are null. No settings loaded.");
                    allSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();
                }
                return allSettings;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error loading all service settings: {ex.Message}");
                throw new Exception("Error loading all service settings:", ex);
            }
        }

        public static void CheckServiceNameAndLogError(ServiceSettingsDto serviceSettings)
        {
            if (string.IsNullOrEmpty(serviceSettings.ServiceName))
            {
                _logger.Error("Service name cannot be null or empty.");
                return;
            }
        }

        private static void UpdateServiceSettings(Dictionary<string, Dictionary<string, ServiceSettingsDto>> allSettings, string serviceKey, ServiceSettingsDto serviceSettings)
        {
            string categoryName = GetCategoryName(serviceKey);
            if (!allSettings.ContainsKey(categoryName))
            {
                allSettings[categoryName] = new Dictionary<string, ServiceSettingsDto>();
                _logger.Information($"Added new category '{categoryName}' to settings.");
            }
            allSettings[categoryName][serviceKey] = serviceSettings;
        }
        internal static string GetCategoryName(string serviceName)
        {
            return serviceName.Contains("Service") ? "Services" : "WebApis";
        }

    }
}
