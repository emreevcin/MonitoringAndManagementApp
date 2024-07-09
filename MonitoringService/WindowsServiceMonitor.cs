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

        public void MonitorService(string serviceName, ServiceSettingsDto settings)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);

                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    _logCatcher.Warning($"{settings.ServiceName} downed. Attempting to restart.");

                    if (settings.NumberOfRuns > 0)
                    {
                        service.Start();
                        settings.NumberOfRuns--;

                        int waitTime = Math.Min(settings.MonitorInterval / 2, 2000);
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(waitTime));

                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            _logCatcher.Information($"{settings.ServiceName} started.");
                        }
                        else
                        {
                            _logCatcher.Error($"{settings.ServiceName} failed to run.");
                        }
                    }
                    else
                    {
                        _logCatcher.Error($"{settings.ServiceName} downed. Maximum restart attempts exceeded.");
                    }
                }
                else
                {
                    _logCatcher.Information($"{settings.ServiceName} is running.");
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
