using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.ServiceProcess;
using System.Timers;
using Util;

namespace FileWatcherService
{
    internal static class Program
    {

        private static string serviceName = "FileWatcherService";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            string path = System.Configuration.ConfigurationManager.AppSettings["Path"];

            ILogger configuration = ConfigureLogger();
            
            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService(configuration,path)
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static ILogger ConfigureLogger()
        {
            string logLevelString = LogLevel.GetLogLevel(serviceName);
            LogEventLevel logLevel = LogLevel.ConvertToLogEventLevel(logLevelString);//quick watch ve stringi nasıl geliyor ona bakılacak.

            return new LoggerConfiguration().ReadFrom.AppSettings().MinimumLevel.Is(logLevel).Enrich.FromLogContext().CreateLogger();
        }
    }
}
