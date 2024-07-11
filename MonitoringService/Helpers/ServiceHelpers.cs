using Microsoft.Web.Administration;
using Serilog;
using System;
using System.ServiceProcess;
using Util;

namespace MonitoringService.Helpers
{
    public static class ServiceHelpers
    {
        public static void CheckAndRestartAppPool(ApplicationPool appPool, ServiceSettingsDto settings, ILogger _logCatcher)
        {
            SettingsHelper.CheckServiceNameAndLogError(settings);
            string serviceName = settings.ServiceName;

            if (appPool == null)
            {
                _logCatcher.Warning($"Application pool '{serviceName}' not found.");
                return;
            }

            switch (appPool.State)
            {
                case ObjectState.Starting:
                    _logCatcher.Information($"{serviceName} is starting.");
                    break;

                case ObjectState.Started:
                    _logCatcher.Information($"{serviceName} is running.");
                    break;

                case ObjectState.Stopping:
                    _logCatcher.Information($"{serviceName} is stopping.");
                    break;

                case ObjectState.Stopped:
                    _logCatcher.Warning($"{serviceName} downed. Attempting to restart.");
                    TryRestartAppPool(appPool, settings, _logCatcher);
                    break;

                case ObjectState.Unknown:
                    _logCatcher.Warning($"{serviceName} state is unknown.");
                    break;

                default:
                    _logCatcher.Warning($"{serviceName} has an unexpected state.");
                    break;
            }
        }

        private static void TryRestartAppPool(ApplicationPool appPool, ServiceSettingsDto settings, ILogger _logCatcher)
        {
            SettingsHelper.CheckServiceNameAndLogError(settings);
            string serviceName = settings.ServiceName;

            try
            {
                if (settings.NumberOfRuns > 0)
                {
                    appPool.Start();
                    settings.NumberOfRuns--;

                    if (appPool.State == ObjectState.Started)
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

        public static void CheckAndRestartWindowsService(ServiceController service, ServiceSettingsDto settings, ServiceControllerStatus status, ILogger _logCatcher)
        {
            SettingsHelper.CheckServiceNameAndLogError(settings);
            string serviceName = settings.ServiceName;

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
                    TryRestartWindowsService(service, settings, _logCatcher);
                    break;

                case ServiceControllerStatus.StopPending:
                    _logCatcher.Information($"{serviceName} stop pending.");
                    break;

                default:
                    _logCatcher.Warning($"{serviceName} has an unexpected status: {status}");
                    break;
            }
        }

        private static void TryRestartWindowsService(ServiceController service, ServiceSettingsDto settings, ILogger _logCatcher)
        {
            SettingsHelper.CheckServiceNameAndLogError(settings);
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
