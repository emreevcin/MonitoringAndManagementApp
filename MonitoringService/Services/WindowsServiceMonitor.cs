using MonitoringService.Helpers;
using Serilog;
using System;
using System.ServiceProcess;
using Util;

namespace MonitoringService
{
    internal class WindowsServiceMonitor : IServiceMonitor
    {
        private readonly ILogger _logCatcher;

        public WindowsServiceMonitor(ILogger logCatcher)
        {
            _logCatcher = logCatcher;
        }

        public void MonitorService(ServiceSettingsDto settings)
        {
            try
            {
                SettingsHelper.CheckServiceNameAndLogError(settings);

                string serviceName = settings.ServiceName;

                ServiceController service = new ServiceController(serviceName);
                ServiceControllerStatus status = service.Status;

                ServiceHelpers.CheckAndRestartWindowsService(service, settings, status, _logCatcher);
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
