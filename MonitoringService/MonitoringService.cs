using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using MonitoringService.Interfaces;
using MonitoringService.Wrappers;
using Serilog;
using Serilog.Core;
using Util;

namespace MonitoringService
{
    public partial class MonitoringService : ServiceBase
    {
        private readonly ILogger _logCatcher;
        private readonly Dictionary<string, IServiceMonitor> _serviceMonitors;
        private readonly ISettingsRepository _settingsHelper;
        private readonly IServiceController _serviceController;

        public MonitoringService(ILogger logCatcher, Dictionary<string, IServiceMonitor> serviceMonitors, ISettingsRepository settingsHelper, IServiceController serviceController)
        {
            InitializeComponent();
            _logCatcher = logCatcher ?? throw new ArgumentNullException(nameof(logCatcher));
            _serviceMonitors = serviceMonitors ?? throw new ArgumentNullException(nameof(serviceMonitors));
            _settingsHelper = settingsHelper ?? throw new ArgumentNullException(nameof(settingsHelper));
            _serviceController = serviceController ?? throw new ArgumentNullException(nameof(serviceController));
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

        public void StartMonitoring()
        {
            try
            {
                var servicesToMonitor = _settingsHelper.LoadAllSettings();
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
                        ServiceSettingsDto settings = serviceEntry.Value;

                        Timer timer = new Timer
                        {
                            Interval = settings.MonitorInterval * 1000,
                            Enabled = true
                        };
                        timer.Elapsed += (sender, e) => serviceMonitor.MonitorService(settings);
                        timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                _logCatcher.Error($"Error loading service settings: {ex.Message}");
            }
        }
    }
}