using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Util
{
    public static class LoggerUtil
    {
        private static ILogger _logger;

        public static ILogger ConfigureLogger(string configType, LogEventLevel logLevel = Constants.DefaultLogLevel, IConfiguration configuration = null)
        {
            switch (configType)
            {
                case "WebApi":
                    _logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .MinimumLevel.Is(logLevel)
                        .Enrich.FromLogContext()
                        .CreateLogger();
                    break;

                case "App.Config":
                    _logger = new LoggerConfiguration()
                        .ReadFrom.AppSettings()
                        .MinimumLevel.Is(logLevel)
                        .Enrich.FromLogContext()
                        .CreateLogger();
                    break;

                case "Util":
                    _logger = new LoggerConfiguration()
                    .WriteTo.File(
                        path: Constants.logFilePath,
                        rollingInterval: RollingInterval.Day, 
                        shared: true)
                    .MinimumLevel.Warning()
                    .Enrich.FromLogContext()
                    .CreateLogger();
                    break;

                default:
                    throw new ArgumentException("Unsupported configuration type");
            }
            return _logger;
        }

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
                _logger.Warning($"LogLevel not found for service '{serviceName}'. Using default: {defaultLogLevel}");
                return defaultLogLevel;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error getting log level: {ex.Message}");
                throw new Exception($"Error getting log level: {ex.Message}");
            }
        }

        public static void CheckServiceNameAndLogError(ServiceSettingsDto serviceSettings)
        {
            if (string.IsNullOrEmpty(serviceSettings.ServiceName))
            {
                _logger.Error("Service name cannot be null or empty.");
                return;
            }
        }
    }
}
