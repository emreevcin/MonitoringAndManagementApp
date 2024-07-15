using Serilog;
using Microsoft.Web.Administration;
using System;
using Util;
using MonitoringService.Helpers;
using MonitoringService.Wrappers;

namespace MonitoringService
{
    public class IISServiceMonitor : IServiceMonitor
    {
        private readonly ILogger _logCatcher;

        public IISServiceMonitor(ILogger logCatcher)
        {
            _logCatcher = logCatcher;
        }

        public void MonitorService(ServiceSettingsDto settings)
        {
            SettingsHelper.CheckServiceNameAndLogError(settings);

            string serviceName = settings.ServiceName;
                
            try
            {
                using (var serverManager = new ServerManager())
                {
                    var appPool = serverManager.ApplicationPools[serviceName];
                    var appPoolWrapper = new ApplicationPoolWrapper(appPool);

                    ServiceHelpers.CheckAndRestartAppPool(appPoolWrapper, settings, _logCatcher);
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
    }
}
