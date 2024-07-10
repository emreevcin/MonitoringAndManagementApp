using System.ServiceProcess;
using Util;
using Util.Generics;

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

            var serviceSettings = SettingsHelper.LoadServiceSettings(_serviceName);

            var configuration = LoggerUtil.ConfigureLogger(LoggerConfigurationType.AppConfig, serviceSettings.LogLevel);
            string path = serviceSettings.FolderPath;

            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService(configuration,path)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
