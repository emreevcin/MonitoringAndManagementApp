using MonitoringService;
using Moq;
using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using Serilog;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MonitoringService.Interfaces;
using Util;
using Util.Generics;

namespace MaMApp.Test.Services.Tests
{
    public class WindowsServiceTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<IServiceController> _mockServiceControllerWrapper;
        private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
        private readonly Mock<IServiceScope> _mockScope;
        private readonly ServiceSettingsDto _settings;

        public WindowsServiceTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockServiceControllerWrapper = new Mock<IServiceController>();
            _mockScopeFactory = new Mock<IServiceScopeFactory>();
            _mockScope = new Mock<IServiceScope>();

            _mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
                                .Returns(_mockScopeFactory.Object);

            _mockScopeFactory.Setup(sf => sf.CreateScope())
                             .Returns(_mockScope.Object);

            _mockScope.Setup(s => s.ServiceProvider)
                      .Returns(_mockServiceProvider.Object);

            _mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceController)))
                                .Returns(_mockServiceControllerWrapper.Object);

            _settings = SettingsHelper.LoadServiceSettings(Enum.GetName(typeof(ServiceNames), ServiceNames.FileWatcherService));
        }

        [Fact]
        public void MonitorService_ShouldNotRestartService_WhenServiceIsRunning()
        {
            // Arrange
            _mockServiceControllerWrapper.SetupGet(sc => sc.Status).Returns(ServiceControllerStatus.Running);

            var monitoringService = new WindowsServiceMonitor(_mockLogger.Object, _mockServiceProvider.Object);

            // Act
            monitoringService.MonitorService(_settings);

            Task.Delay(1000).Wait();

            // Assert
            _mockServiceControllerWrapper.Verify(sc => sc.Start(), Times.Never);
            _mockLogger.Verify(l => l.Information($"{_settings.ServiceName} is running."), Times.Once);
        }
    }
}