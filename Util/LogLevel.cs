using System;
using Serilog;
using Serilog.Events;


namespace Util
{
    public static class LogLevel
    {
        private static readonly ILogger logger = SerilogHelper.GetLogger();

        public static string GetLogLevel(string serviceName)
        {
            string defaultLogLevel = "information"; 
            try
            {
                dynamic serviceSettings = JsonHelper.LoadServiceSettings(serviceName);
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

        public static LogEventLevel ConvertToLogEventLevel(string logLevel)
        {
            switch (logLevel.ToLower())
            {
                case "verbose":
                    return LogEventLevel.Verbose;
                case "debug":
                    return LogEventLevel.Debug;
                case "warning":
                    return LogEventLevel.Warning;
                case "error":
                    return LogEventLevel.Error;
                case "fatal":
                    return LogEventLevel.Fatal;
                case "information":
                case "info":
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
