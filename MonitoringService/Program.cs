using MonitoringService.Interfaces;
using MonitoringService.Wrappers;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Util;
using Util.Generics;
using Microsoft.Extensions.DependencyInjection;

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

            var serviceProvider = ConfigureServices();

            var logCatcher = LoggerUtil.ConfigureLogger(LoggerConfigurationType.AppConfig);
            var serviceMonitors = new Dictionary<string, IServiceMonitor>
            {
                { Enum.GetName(typeof(SettingsCategories), SettingsCategories.Services), new WindowsServiceMonitor(logCatcher, serviceProvider) },
                { Enum.GetName(typeof(SettingsCategories), SettingsCategories.WebApis), new IISServiceMonitor(logCatcher) }
            };
            var settingsHelper = new JsonSettingsRepository(Constants.settingsFilePath);
            var serviceController = new ServiceControllerWrapper(Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService));
                
            ServicesToRun = new ServiceBase[]
            {
                new MonitoringService(logCatcher, serviceMonitors, settingsHelper, serviceController)
            };
            ServiceBase.Run(ServicesToRun);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register services and wrappers
            services.AddSingleton<IServiceController>(_ => new ServiceControllerWrapper(Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService)));
            // Add other services as needed

            return services.BuildServiceProvider();
        }
    }
}
