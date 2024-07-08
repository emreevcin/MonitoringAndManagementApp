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
        private readonly IServiceSettingsLoader _serviceSettingsLoader;
        private readonly Dictionary<string, IServiceMonitor> _serviceMonitors;

        public MonitoringService(ILogger logCatcher, IServiceSettingsLoader serviceSettingsLoader, Dictionary<string, IServiceMonitor> serviceMonitors)
        {
            InitializeComponent();
            _logCatcher = logCatcher;
            _serviceSettingsLoader = serviceSettingsLoader;
            _serviceMonitors = serviceMonitors;
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
            var servicesToMonitor = _serviceSettingsLoader.LoadServiceSettings();

            foreach (var categoryEntry in servicesToMonitor)
            {
                string categoryName = categoryEntry.Key;
                var servicesInCategory = categoryEntry.Value;

                if (!_serviceMonitors.ContainsKey(categoryName))
                {
                    _logCatcher.Warning($"Unknown category '{categoryName}' encountered. No monitoring action taken for services in this category.");
                    continue;
                }

                var serviceMonitor = _serviceMonitors[categoryName];

                foreach (var serviceEntry in servicesInCategory)
                {
                    string serviceName = serviceEntry.Key;
                    ServiceSettings settings = serviceEntry.Value;

                    Timer timer = new Timer
                    {
                        Interval = settings.MonitorInterval * 1000,
                        Enabled = true
                    };
                    timer.Elapsed += (sender, e) => serviceMonitor.MonitorService(serviceName, settings);
                    timer.Start();
                }
            }
        }
    }
}