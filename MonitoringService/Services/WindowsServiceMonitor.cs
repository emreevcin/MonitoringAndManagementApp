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

                LogServiceStatus(serviceName, status);

                if (status == ServiceControllerStatus.Stopped)
                    TryRestartService(service, settings);
                
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

        private void LogServiceStatus(string serviceName, ServiceControllerStatus status)
        {
            switch (status)
            {
                case ServiceControllerStatus.ContinuePending:
                    _logCatcher.Information($"{serviceName} continue pending.");
                    break;

                case ServiceControllerStatus.Paused:
                    _logCatcher.Information($"{serviceName} is paused.");
                    break;

                case ServiceControllerStatus.PausePending:
                    _logCatcher.Information($"{serviceName} pause pending.");
                    break;

                case ServiceControllerStatus.Running:
                    _logCatcher.Information($"{serviceName} is running.");
                    break;

                case ServiceControllerStatus.StartPending:
                    _logCatcher.Information($"{serviceName} start pending.");
                    break;

                case ServiceControllerStatus.Stopped:
                    _logCatcher.Warning($"{serviceName} is stopped.");
                    break;

                case ServiceControllerStatus.StopPending:
                    _logCatcher.Information($"{serviceName} stop pending.");
                    break;

                default:
                    _logCatcher.Warning($"{serviceName} has an unexpected status: {status}");
                    break;
            }
        }

        private void TryRestartService(ServiceController service, ServiceSettingsDto settings)
        {
            string serviceName = settings.ServiceName;

            try
            {
                _logCatcher.Warning($"{serviceName} downed. Attempting to restart.");

                if (settings.NumberOfRuns > 0)
                {
                    service.Start();
                    settings.NumberOfRuns--;

                    int waitTime = Math.Min(settings.MonitorInterval / 2, 2000);
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(waitTime));

                    if (service.Status == ServiceControllerStatus.Running)
                        _logCatcher.Information($"{serviceName} started.");
                    else
                        _logCatcher.Error($"{serviceName} failed to run.");
                }
                else
                    _logCatcher.Error($"{serviceName} downed. Maximum restart attempts exceeded.");
            }
            catch (InvalidOperationException ex)
            {
                _logCatcher.Error($"Invalid operation when restarting {serviceName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error restarting {serviceName}: {ex.Message}");
            }
        }
    }
}
