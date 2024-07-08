using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;

namespace Util
{
    public class JsonSettingsRepository : ISettingsRepository
    {
        private readonly string settingsFilePath = Constants.settingsFilePath;
        private static readonly ILogger logger = SerilogHelper.GetLogger();

        public JsonSettingsRepository(string filePath)
        {
            settingsFilePath = filePath;
        }
        public JObject LoadAllSettings()
        {
            try
            {
                if (File.Exists(settingsFilePath))
                {
                    string json = File.ReadAllText(settingsFilePath);
                    return JObject.Parse(json);
                }
                else
                {
                    Log.Warning($"Settings file not found at {settingsFilePath}");
                    return HandleMissingSettingsFile();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error loading settings: {ex.Message}");
                throw new Exception("Error loading settings", ex);
            }
        }

        public void SaveAllSettings(dynamic allSettings)
        {
            try
            {
                string json = JsonConvert.SerializeObject(allSettings, Formatting.Indented);
                File.WriteAllText(settingsFilePath, json);
            }
            catch (Exception ex)
            {
                logger.Error($"Error saving settings: {ex.Message}");
                throw new Exception("Error saving settings", ex);
            }
        }

        private dynamic HandleMissingSettingsFile()
        {
            var defaultSettings = new JObject();
            SaveAllSettings(defaultSettings);
            Log.Warning($"Created default settings file at {settingsFilePath}");
            return defaultSettings;
        }
    }
}
