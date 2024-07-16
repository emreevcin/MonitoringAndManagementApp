using Xunit;
using Moq;
using System.Collections.Generic;
using Serilog;
using Util;
using MonitoringService;
using System.Timers;
using MonitoringService.Interfaces;
using Util.Generics;

namespace MaMApp.Test.Services.Tests
{
    public class MonitoringServiceTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IServiceMonitor> _mockServiceMonitor;
        private readonly Dictionary<string, IServiceMonitor> _serviceMonitors;
        private readonly Mock<ISettingsRepository> _mockSettingsHelper;
        private readonly Mock<IServiceController> _mockServiceController;
        private readonly Mock<IServiceProvider> _mockServiceProvider;

        public MonitoringServiceTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockServiceMonitor = new Mock<IServiceMonitor>();
            _mockSettingsHelper = new Mock<ISettingsRepository>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _serviceMonitors = new Dictionary<string, IServiceMonitor>
            {
                { Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService), _mockServiceMonitor.Object },
                { Enum.GetName(typeof(ServiceNames), ServiceNames.LogoWebApi), _mockServiceMonitor.Object }
            };
            _mockServiceController = new Mock<IServiceController>();

            _mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceMonitor)))
                                .Returns(_mockServiceMonitor.Object);
        }

        [Fact]
        public void StartMonitoringService_ShouldInvokeMonitorService()
        {
            // Arrange
            var settings = SettingsHelper.LoadServiceSettings(Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService));

            var servicesToMonitor = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
            {
                { Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService), new Dictionary<string, ServiceSettingsDto> { { Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService), settings } } }
            };

            _mockSettingsHelper.Setup(s => s.LoadAllSettings()).Returns(servicesToMonitor);

            var monitoringService = new MonitoringService.MonitoringService(_mockLogger.Object, _serviceMonitors, _mockSettingsHelper.Object, _mockServiceController.Object);

            // Act
            monitoringService.StartMonitoring();

            System.Threading.Thread.Sleep(6000);

            // Assert
            var monitorServiceCalls = _mockServiceMonitor.Invocations
                .Count(inv => inv.Method.Name == nameof(_mockServiceMonitor.Object.MonitorService));

            Assert.True(monitorServiceCalls > 0, "MonitorService should have been called at least once.");
        }

        [Fact]
        public void StartMonitoring_ShouldInvokeMonitorService_ForLogoWebApi()
        {
            // Arrange
            var settings = SettingsHelper.LoadServiceSettings(Enum.GetName(typeof(ServiceNames), ServiceNames.LogoWebApi));

            var servicesToMonitor = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
            {
                { Enum.GetName(typeof(ServiceNames), ServiceNames.LogoWebApi), new Dictionary<string, ServiceSettingsDto> { { Enum.GetName(typeof(ServiceNames), ServiceNames.LogoWebApi), settings } } }
            };

            _mockSettingsHelper.Setup(s => s.LoadAllSettings()).Returns(servicesToMonitor);

            var monitoringService = new MonitoringService.MonitoringService(_mockLogger.Object, _serviceMonitors, _mockSettingsHelper.Object, _mockServiceController.Object);

            // Act
            monitoringService.StartMonitoring();

            System.Threading.Thread.Sleep(6000);

            // Assert
            var monitorServiceCalls = _mockServiceMonitor.Invocations
                .Count(inv => inv.Method.Name == nameof(_mockServiceMonitor.Object.MonitorService));

            Assert.True(monitorServiceCalls > 0, "MonitorService should have been called at least once.");
        }
    }
}
