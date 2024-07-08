using System.Configuration;

namespace SettingsApplication
{
    internal class AppConfigUpdater : IConfigUpdater
    {
        public void UpdateAppConfigLogLevel(string serviceName, string logLevel, string folderPath, string appConfigPath)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = appConfigPath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            const string logLevelKey = "serilog:minimum-level";
            const string pathKey = "Path";

            if (config.AppSettings.Settings[logLevelKey] != null)
            {
                config.AppSettings.Settings[logLevelKey].Value = logLevel;
            }
            else
            {
                config.AppSettings.Settings.Add(logLevelKey, logLevel);
            }

            if (config.AppSettings.Settings[pathKey] != null)
            {
                config.AppSettings.Settings[pathKey].Value = folderPath;
            }
            else
            {
                config.AppSettings.Settings.Add(pathKey, folderPath);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
