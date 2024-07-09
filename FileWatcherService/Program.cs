using Serilog;
using System.ServiceProcess;
using Util;

namespace FileWatcherService
{
    internal static class Program
    {

        private readonly static string _serviceName = "FileWatcherService";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            var configuration = ConfigureLogger();
            string path = ServiceSettingManager.GetServicePath(_serviceName);

            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService(configuration,path)
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static ILogger ConfigureLogger()
        {
            string logLevelString = LogLevel.GetLogLevel(_serviceName);
            var logLevel = LogLevel.ConvertToLogEventLevel(logLevelString);

            return new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .MinimumLevel.Is(logLevel)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
