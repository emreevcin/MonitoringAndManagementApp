using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using Microsoft.Web.Administration;
using Serilog;
using Util;

namespace MonitoringService
{
    public partial class MonitoringService : ServiceBase
    {
        private readonly ILogger _logCatcher;
        private readonly Dictionary<string, Dictionary<string, ServiceSettings>> _servicesToMonitor;

        public MonitoringService(ILogger logCatcher)
        {
            InitializeComponent();
            _servicesToMonitor = LoadServiceSettings();
            _logCatcher = logCatcher;
        }

        protected override void OnStart(string[] args)
        {
            _logCatcher.Information("Monitoring Service started.");
            StartMonitoring();
        }

        protected override void OnStop()
        {
            _logCatcher.Information("Monitoring Service stopped.");
        }

        private void StartMonitoring()
        {
            foreach (var categoryEntry in _servicesToMonitor)
            {
                string categoryName = categoryEntry.Key;
                var servicesInCategory = categoryEntry.Value;

                foreach (var serviceEntry in servicesInCategory)
                {
                    string serviceName = serviceEntry.Key;
                    ServiceSettings settings = serviceEntry.Value;

                    Timer timer = new Timer();

                    if (categoryName == "Services")
                    {
                        timer.Elapsed += (sender, e) => MonitorService(serviceName, settings);
                    }
                    else if (categoryName == "WebApis")
                    {
                        timer.Elapsed += (sender, e) => MonitorIISApplication(serviceName, settings);
                    }
                    else
                    {
                        _logCatcher.Warning($"Unknown category '{categoryName}' encountered. No monitoring action taken for service '{serviceName}'.");
                        continue;
                    }

                    timer.Interval = settings.MonitorInterval * 1000; // Convert seconds to milliseconds
                    timer.Enabled = true;
                    timer.Start();
                }
            }
        }

    private void MonitorIISApplication(string applicationName, ServiceSettings settings)
        {
            try
            {
                using (var serverManager = new ServerManager())
                {
                    ApplicationPool appPool = serverManager.ApplicationPools[applicationName];

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

        private void MonitorService(string serviceName, ServiceSettings settings)
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

                        int waitTime = Math.Min(settings.MonitorInterval * 200, 2000); // Cap the wait time at 2000 ms (2 seconds)
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
                    _logCatcher.Information($"{settings.ServiceName} is working.");
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

        private Dictionary<string, Dictionary<string, ServiceSettings>> LoadServiceSettings()
        {
            Dictionary<string, Dictionary<string, ServiceSettings>> serviceSettings = new Dictionary<string, Dictionary<string, ServiceSettings>>();

            try
            {
                serviceSettings = JsonHelper.LoadAllServiceSettings();
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error loading service settings: {ex.Message}");
            }

            return serviceSettings;
        }

    }
}