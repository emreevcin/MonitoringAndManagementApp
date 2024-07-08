using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Dictionary<string, Dictionary<string, ServiceSettings>> LoadServiceSettings()
        {
            try
            {
                return JsonHelper.LoadAllServiceSettings();
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error loading service settings: {ex.Message}");
                return new Dictionary<string, Dictionary<string, ServiceSettings>>();
            }
        }
    }
}
