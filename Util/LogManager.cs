using System;
using Serilog;
using Serilog.Events;

namespace Util
{
    public static class LogManager
    {
        private static readonly ILogger logger = SerilogHelper.GetLogger();

        public static LogEventLevel GetLogLevel(string serviceName)
        {
            LogEventLevel defaultLogLevel = Constants.DefaultLogLevel;
            try
            {
                dynamic serviceSettings = SettingsHelper.LoadServiceSettings(serviceName);
                if (serviceSettings != null && serviceSettings.LogLevel != null)
                {
                    return serviceSettings.LogLevel;
                }
                logger.Warning($"LogLevel not found for service '{serviceName}'. Using default: {defaultLogLevel}");
                return defaultLogLevel;
            }
            catch (Exception ex)
            {
                logger.Error($"Error getting log level: {ex.Message}");
                throw new Exception($"Error getting log level: {ex.Message}");
            }
        }

        public static void CheckServiceNameAndLogError(ServiceSettingsDto serviceSettings)
        {
            if (string.IsNullOrEmpty(serviceSettings.ServiceName))
            {
                logger.Error("Service name cannot be null or empty.");
                return;
            }
        }
    }
}