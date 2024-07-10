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

            var configuration = LoggerUtil.ConfigureLogger("App.Config", LoggerUtil.GetLogLevel(_serviceName));
            string path = ServiceSettingManager.GetServicePath(_serviceName);

            ServicesToRun = new ServiceBase[]
            {
                new FileWatcherService(configuration,path)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
