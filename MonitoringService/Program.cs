using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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

            var logCatcher = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();
            var serviceSettingsLoader = new JsonServiceSettingsLoader(logCatcher);
            var serviceMonitors = new Dictionary<string, IServiceMonitor>
            {
                { "Services", new WindowsServiceMonitor(logCatcher) },
                { "WebApis", new IISServiceMonitor(logCatcher) }
            };
                
            ServicesToRun = new ServiceBase[]
            {
                new MonitoringService(logCatcher, serviceSettingsLoader, serviceMonitors)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
