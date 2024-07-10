using Serilog;
using Microsoft.Web.Administration;
using System;
using Util;

namespace MonitoringService
{
    internal class IISServiceMonitor : IServiceMonitor
    {
        private readonly ILogger _logCatcher;

        public IISServiceMonitor(ILogger logCatcher)
        {
            _logCatcher = logCatcher;
        }

        public void MonitorService(ServiceSettingsDto settings)
        {
            LoggerUtil.CheckServiceNameAndLogError(settings);

            string serviceName = settings.ServiceName;
                
            try
            {
                using (var serverManager = new ServerManager())
                {
                    ApplicationPool appPool = serverManager.ApplicationPools[serviceName];

                    CheckAndRestartAppPool(appPool, settings, serviceName);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logCatcher.Error($"Invalid operation when checking {serviceName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error checking {serviceName}: {ex.Message}");
            }
        }

        public void CheckAndRestartAppPool(ApplicationPool appPool, ServiceSettingsDto settings, string serviceName)
        {
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
                    break;

                case ObjectState.Unknown:
                    _logCatcher.Warning($"{serviceName} state is unknown.");
                    break;

                default:
                    _logCatcher.Warning($"{serviceName} has an unexpected state.");
                    break;
            }
        }

    }
}
