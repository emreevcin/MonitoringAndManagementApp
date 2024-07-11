using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Util;
using Util.Generics;

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

            var logCatcher = LoggerUtil.ConfigureLogger(LoggerConfigurationType.AppConfig);
            var serviceMonitors = new Dictionary<string, IServiceMonitor>
            {
                { Enum.GetName(typeof(SettingsCategories), SettingsCategories.Services), new WindowsServiceMonitor(logCatcher) },
                { Enum.GetName(typeof(SettingsCategories), SettingsCategories.WebApis), new IISServiceMonitor(logCatcher) }
            };
                
            ServicesToRun = new ServiceBase[]
            {
                new MonitoringService(logCatcher, serviceMonitors)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
