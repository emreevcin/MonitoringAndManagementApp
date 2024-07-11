using Serilog.Events;
using System.Configuration;
using System;


namespace Util.AppConfigSettings
{
    public class AppConfigUpdater
    {
        public void UpdateAppConfigLogLevel(string serviceName, LogEventLevel logLevel, string appConfigPath)
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = appConfigPath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            const string logLevelKey = "serilog:minimum-level";

            if (config.AppSettings.Settings[logLevelKey] != null)
            {
                config.AppSettings.Settings[logLevelKey].Value = Enum.GetName(typeof(LogEventLevel),logLevel);
            }
            else
            {
                config.AppSettings.Settings.Add(logLevelKey, Enum.GetName(typeof(LogEventLevel), logLevel));
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
