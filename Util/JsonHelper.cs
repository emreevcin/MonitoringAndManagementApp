using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace Util
{
    public static class JsonHelper
    {
        private static string settingsFilePath = Constants.settingsFilePath;
        private static readonly ILogger logger = SerilogHelper.GetLogger();
        private static readonly ISettingsRepository settingsRepository = new JsonSettingsRepository(settingsFilePath);

        public static void SaveServiceSettings(string serviceKey, ServiceSettings serviceSettings)
        {
            logger.Information($"Saving settings for {serviceKey}");

            try
            {
                dynamic allSettings = settingsRepository.LoadAllSettings() ?? new JObject();
                UpdateServiceSettings(allSettings, serviceKey, serviceSettings);
                settingsRepository.SaveAllSettings(allSettings);
            }
            catch (Exception ex)
            {
                logger.Error($"Error saving service settings: {ex.Message}");
                throw new Exception("Error saving service settings", ex);
            }
        }

        public static ServiceSettings LoadServiceSettings(string serviceKey)
        {
            logger.Information($"Loading settings for {serviceKey}");

            try
            {
                dynamic allSettings = settingsRepository.LoadAllSettings();
                if (allSettings != null)
                {
                    string categoryName = serviceKey.Contains("Service") ? "Services" : "WebApis";
                    if (allSettings.ContainsKey(categoryName) && allSettings[categoryName][serviceKey] != null)
                    {
                        return allSettings[categoryName][serviceKey].ToObject<ServiceSettings>();
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

        public static Dictionary<string, Dictionary<string, ServiceSettings>> LoadAllServiceSettings()
        {
            try
            {
                dynamic allSettings = settingsRepository.LoadAllSettings();
                Dictionary<string, Dictionary<string, ServiceSettings>> categorizedSettings = new Dictionary<string, Dictionary<string, ServiceSettings>>();

                if (allSettings != null)
                {
                    foreach (var category in allSettings.Properties())
                    {
                        string categoryName = category.Name;
                        foreach (var serviceKey in category.Value.Children<JProperty>())
                        {
                            ServiceSettings settings = serviceKey.Value.ToObject<ServiceSettings>();

                            if (!categorizedSettings.ContainsKey(categoryName))
                            {
                                categorizedSettings[categoryName] = new Dictionary<string, ServiceSettings>();
                            }

                            categorizedSettings[categoryName].Add(serviceKey.Name, settings);
                        }
                    }
                }
                else
                {
                    logger.Warning("All settings are null. No settings loaded.");
                }

                return categorizedSettings;
            }
            catch (Exception ex)
            {
                logger.Error($"Error loading all service settings: {ex.Message}");
                throw new Exception("Error loading all service settings:", ex);
            }
        }

        private static void UpdateServiceSettings(dynamic allSettings, string serviceKey, ServiceSettings serviceSettings)
        {
            string categoryName = serviceKey.Contains("Service") ? "Services" : "WebApis";
            if (!allSettings.ContainsKey(categoryName))
            {
                allSettings[categoryName] = new JObject();
                logger.Information($"Added new category '{categoryName}' to settings.");
            }
            allSettings[categoryName][serviceKey] = JObject.FromObject(serviceSettings);
        }

        private static ServiceSettings CreateDefaultServiceSettings(string serviceKey)
        {
            logger.Information($"Creating default settings for {serviceKey}");
            return ServiceSettingsFactory.CreateDefaultServiceSettings(serviceKey);
        }
    }
}
