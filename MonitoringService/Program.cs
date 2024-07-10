using Serilog;
using System.Collections.Generic;
using System.ServiceProcess;
using Util;

namespace MonitoringService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            ServiceBase[] ServicesToRun;

            //var logCatcher = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            var logCatcher = LoggerUtil.ConfigureLogger("App.Config");
            var serviceMonitors = new Dictionary<string, IServiceMonitor>
            {
                { "Services", new WindowsServiceMonitor(logCatcher) },
                { "WebApis", new IISServiceMonitor(logCatcher) }
            };
                
            ServicesToRun = new ServiceBase[]
            {
                new MonitoringService(logCatcher, serviceMonitors)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
