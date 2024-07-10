using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Util.Generics;

namespace Util
{
    public static class LoggerUtil
    {
        private static ILogger _logger;

        public static ILogger ConfigureLogger(LoggerConfigurationType configType, LogEventLevel logLevel = Constants.DefaultLogLevel, IConfiguration configuration = null)
        {
            switch (configType)
            {
                case LoggerConfigurationType.WebApi:
                    _logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .MinimumLevel.Is(logLevel)
                        .Enrich.FromLogContext()
                        .CreateLogger();
                    break;

                case LoggerConfigurationType.AppConfig:
                    _logger = new LoggerConfiguration()
                        .ReadFrom.AppSettings()
                        .MinimumLevel.Is(logLevel)
                        .Enrich.FromLogContext()
                        .CreateLogger();
                    break;

                case LoggerConfigurationType.Util:
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
    }
}
