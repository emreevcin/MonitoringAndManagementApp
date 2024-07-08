using System;
using Serilog.Events;


namespace Util
{
    public static class LogLevel
    {
        public static string GetLogLevel(string serviceName)
        {
            try
            {
                dynamic serviceSettings = JsonHelper.LoadServiceSettings(serviceName);
                if (serviceSettings != null && serviceSettings.LogLevel != null)
                {
                    return serviceSettings.LogLevel;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting log level: {ex.Message}"); // log dosyasına bas MonitoringService
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
