using System;
using Serilog;
using Serilog.Events;

namespace Util
{
    public static class LogLevel
    {
        private static readonly ILogger logger = SerilogHelper.GetLogger();

        public enum LogLevelEnum
        {
            Verbose,
            Debug,
            Warning,
            Error,
            Fatal,
            Information
        }

        public static LogEventLevel GetLogEventLevel(string serviceName)
        {
            string logLevel = GetLogLevel(serviceName);
            return ConvertToLogEventLevel(logLevel);
        }

        private static string GetLogLevel(string serviceName)
        {
            string defaultLogLevel = Constants.DefaultLogLevel.ToString();
            try
            {
                dynamic serviceSettings = SettingsJsonHelper.LoadServiceSettings(serviceName);
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

        private static LogEventLevel ConvertToLogEventLevel(string logLevel)
        {
            LogLevelEnum parsedLevel;
            if (Enum.TryParse(logLevel, true, out parsedLevel))
            {
                switch (parsedLevel)
                {
                    case LogLevelEnum.Verbose:
                        return LogEventLevel.Verbose;
                    case LogLevelEnum.Debug:
                        return LogEventLevel.Debug;
                    case LogLevelEnum.Warning:
                        return LogEventLevel.Warning;
                    case LogLevelEnum.Error:
                        return LogEventLevel.Error;
                    case LogLevelEnum.Fatal:
                        return LogEventLevel.Fatal;
                    case LogLevelEnum.Information:
                    default:
                        return LogEventLevel.Information;
                }
            }
            else
            {
                logger.Warning($"Unknown log level '{logLevel}'. Using default: {LogLevelEnum.Information}");
                return LogEventLevel.Information;
            }
        }
    }
}
