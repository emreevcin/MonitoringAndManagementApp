using Serilog;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void MonitorService(string serviceName, ServiceSettings settings)
        {
            try
            {
                using (var serverManager = new ServerManager())
                {
                    ApplicationPool appPool = serverManager.ApplicationPools[serviceName];

                    if (appPool != null)
                    {
                        if (appPool.State == ObjectState.Stopped)
                        {
                            _logCatcher.Warning($"{settings.ServiceName} downed. Attempting to restart.");

                            if (settings.NumberOfRuns > 0)
                            {
                                appPool.Start();
                                settings.NumberOfRuns--;

                                if (appPool.State == ObjectState.Started)
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
                            _logCatcher.Information($"{settings.ServiceName} is working.");
                        }
                    }
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
