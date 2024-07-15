using Xunit;
using Moq;
using System.Collections.Generic;
using Serilog;
using Util;
using MonitoringService;
using System.Timers;
using MonitoringService.Interfaces;


namespace MaMApp.Test.Services.Tests
{
    public class MonitoringServiceTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IServiceMonitor> _mockServiceMonitor;
        private readonly Dictionary<string, IServiceMonitor> _serviceMonitors;
        private readonly Mock<ISettingsHelper> _mockSettingsHelper;
        private readonly Mock<IServiceController> _mockServiceController;
        private readonly Mock<IServiceProvider> _mockServiceProvider;

        public MonitoringServiceTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockServiceMonitor = new Mock<IServiceMonitor>();
            _mockSettingsHelper = new Mock<ISettingsHelper>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _serviceMonitors = new Dictionary<string, IServiceMonitor>
            {
                { "FileWatcherService", _mockServiceMonitor.Object },
                { "LogoWebApi", _mockServiceMonitor.Object }
            };
            _mockServiceController = new Mock<IServiceController>();

            _mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceMonitor)))
                                .Returns(_mockServiceMonitor.Object);
        }

        [Fact]
        public void StartMonitoringService_ShouldInvokeMonitorService()
        {
            // Arrange
            var settings = new ServiceSettingsDto
            {
                ServiceName = "FileWatcherService",
                MonitorInterval = 5,
                NumberOfRuns = 3,
                LogLevel = (Serilog.Events.LogEventLevel)2,
                Url = null,
                FolderPath = ".\\"
            };

            var servicesToMonitor = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
            {
                { "FileWatcherService", new Dictionary<string, ServiceSettingsDto> { { "FileWatcherService", settings } } }
            };

            _mockSettingsHelper.Setup(s => s.LoadAllServiceSettings()).Returns(servicesToMonitor);

            var monitoringService = new MonitoringService.MonitoringService(_mockLogger.Object, _serviceMonitors, _mockSettingsHelper.Object, _mockServiceController.Object);

            // Act
            monitoringService.StartMonitoring();

            System.Threading.Thread.Sleep(6000);

            // Assert
            _mockServiceMonitor.Verify(m => m.MonitorService(It.IsAny<ServiceSettingsDto>()), Times.AtLeastOnce);

        }

        [Fact]
        public void StartMonitoring_ShouldInvokeMonitorService_ForLogoWebApi()
        {
            // Arrange
            var settings = new ServiceSettingsDto
            {
                ServiceName = "LogoWebApi",
                MonitorInterval = 5,
                NumberOfRuns = 3,
                LogLevel = (Serilog.Events.LogEventLevel)2,
                Url = "http://localhost:121/swagger/index.html",
                FolderPath = null
            };

            var servicesToMonitor = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
        {
            { "LogoWebApi", new Dictionary<string, ServiceSettingsDto> { { "LogoWebApi", settings } } }
        };

            _mockSettingsHelper.Setup(s => s.LoadAllServiceSettings()).Returns(servicesToMonitor);

            var monitoringService = new MonitoringService.MonitoringService(_mockLogger.Object, _serviceMonitors, _mockSettingsHelper.Object, _mockServiceController.Object);

            // Act
            monitoringService.StartMonitoring();

            System.Threading.Thread.Sleep(6000);

            // Assert
            _mockServiceMonitor.Verify(m => m.MonitorService(It.IsAny<ServiceSettingsDto>()), Times.AtLeastOnce);
        }
    }
}
