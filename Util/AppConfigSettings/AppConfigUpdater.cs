using System.Configuration;


namespace Util.AppConfigSettings
{
    public class AppConfigUpdater : IConfigUpdater
    {
        public void UpdateAppConfigLogLevel(string serviceName, string logLevel, string appConfigPath)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = appConfigPath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            const string logLevelKey = "serilog:minimum-level";

            if (config.AppSettings.Settings[logLevelKey] != null)
            {
                config.AppSettings.Settings[logLevelKey].Value = logLevel;
            }
            else
            {
                config.AppSettings.Settings.Add(logLevelKey, logLevel);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
