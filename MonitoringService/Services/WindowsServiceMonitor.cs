using Microsoft.Extensions.DependencyInjection;
using MonitoringService.Helpers;
using MonitoringService.Interfaces;
using MonitoringService.Wrappers;
using Serilog;
using System;
using System.ServiceProcess;
using Util;

namespace MonitoringService
{
    public class WindowsServiceMonitor : IServiceMonitor
    {
        private readonly ILogger _logCatcher;
        private readonly IServiceProvider _serviceProvider;

        public WindowsServiceMonitor(ILogger logCatcher, IServiceProvider serviceProvider)
        {
            _logCatcher = logCatcher ?? throw new ArgumentNullException(nameof(logCatcher));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void MonitorService(ServiceSettingsDto settings)
        {
            try
            {
                SettingsHelper.CheckServiceNameAndLogError(settings);

                string serviceName = settings.ServiceName;

                using (var scope = _serviceProvider.CreateScope())
                {
                    var serviceController = new ServiceControllerWrapper(serviceName);

                    ServiceHelpers.CheckAndRestartWindowsService(serviceController,settings, _logCatcher);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logCatcher.Error($"Invalid operation when checking {settings.ServiceName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error checking {settings.ServiceName}: {ex.Message}");
            }
        }

    }
}
