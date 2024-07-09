using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Collections.Generic;

namespace Util
{
    public class JsonSettingsRepository : ISettingsRepository
    {
        private readonly string _settingsFilePath = Constants.settingsFilePath;
        private static readonly ILogger logger = SerilogHelper.GetLogger();

        public JsonSettingsRepository(string filePath)
        {
            _settingsFilePath = filePath;
        }
        public Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadAllSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    string json = File.ReadAllText(_settingsFilePath);
                    return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, ServiceSettingsDto>>>(json);
                }
                else
                {
                    logger.Warning($"Settings file not found at {_settingsFilePath}");
                    return HandleMissingSettingsFile();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error loading settings: {ex.Message}");
                throw new Exception("Error loading settings", ex);
            }
        }

        public void SaveAllSettings(Dictionary<string, Dictionary<string, ServiceSettingsDto>> allSettings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(allSettings, Formatting.Indented);
                File.WriteAllText(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                logger.Error($"Error saving settings: {ex.Message}");
                throw new Exception("Error saving settings", ex);
            }
        }

        private Dictionary<string, Dictionary<string, ServiceSettingsDto>> HandleMissingSettingsFile()
        {
            var defaultSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();
            SaveAllSettings(defaultSettings);
            logger.Warning($"Created default settings file at {_settingsFilePath}");
            return defaultSettings;
        }
    }
}
