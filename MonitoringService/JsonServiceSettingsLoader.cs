using Serilog;
using System;
using System.Collections.Generic;
using Util;

namespace MonitoringService
{
    internal class JsonServiceSettingsLoader : IServiceSettingsLoader
    {
        private readonly ILogger _logCatcher;

        public JsonServiceSettingsLoader(ILogger logger)
        {
            _logCatcher = logger;
        }

        public Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadServiceSettings()
        {
            try
            {
                return SettingsJsonHelper.LoadAllServiceSettings();
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error loading service settings: {ex.Message}");
                return new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();
            }
        }
    }
}
