using LogoMockWebApi.Controllers;
using LogoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Administration;
using MonitoringService;
using MonitoringService.Helpers;
using MonitoringService.Interfaces;
using Moq;
using Serilog;
using System;
using Util;
using Util.Generics;
using Xunit;

namespace MaMApp.Test.Services.Tests
{
    public class IISServiceTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly Mock<IApplicationPoolWrapper> _mockAppPool;
        private readonly IISServiceMonitor _iisServiceMonitor;
        private readonly ServiceSettingsDto _settings;

        public IISServiceTests()
        {
            _mockLogger = new Mock<ILogger>();
            _mockAppPool = new Mock<IApplicationPoolWrapper>();
            _iisServiceMonitor = new IISServiceMonitor(_mockLogger.Object);

            _settings = SettingsHelper.LoadServiceSettings(Enum.GetName(typeof(ServiceNames), ServiceNames.LogoWebApi));
        }

        [Fact]
        public void MonitorService_ShouldLogWarningAndInfo_WhenAppPoolIsStopped()
        {
            // Arrange
            _mockAppPool.SetupGet(ap => ap.State).Returns(ObjectState.Stopped);

            // Act
            ServiceHelpers.CheckAndRestartAppPool(_mockAppPool.Object, _settings, _mockLogger.Object);

            // Assert
            _mockLogger.Verify(l => l.Warning($"{_settings.ServiceName} downed. Attempting to restart."), Times.Once);
        }

        [Fact]
        public void MonitorService_ShouldRestartAppPool_WhenStoppedWhileRunning()
        {
            // Arrange
            _mockAppPool.SetupGet(ap => ap.State).Returns(ObjectState.Stopped);
            _mockAppPool.Setup(ap => ap.Start()).Callback(() =>
            {
                _mockAppPool.SetupGet(ap => ap.State).Returns(ObjectState.Started);
            });

            // Act
            ServiceHelpers.CheckAndRestartAppPool(_mockAppPool.Object, _settings, _mockLogger.Object);

            // Assert
            _mockAppPool.Verify(ap => ap.Start(), Times.Once);
            _mockLogger.Verify(l => l.Information($"{_settings.ServiceName} started."), Times.Once);
        }

        [Fact]
        public void MonitorService_ShouldNotRestartAppPool_AfterMaxAttempts()
        {
            // Arrange
            _mockAppPool.SetupGet(ap => ap.State).Returns(ObjectState.Stopped);
            _settings.NumberOfRuns = 0;

            // Act
            ServiceHelpers.CheckAndRestartAppPool(_mockAppPool.Object, _settings, _mockLogger.Object);

            // Assert
            _mockAppPool.Verify(ap => ap.Start(), Times.Never);
            _mockLogger.Verify(l => l.Error($"{_settings.ServiceName} downed. Maximum restart attempts exceeded."), Times.Once);
        }

        [Fact]
        public void Sum_ShouldReturnCorrectResult()
        {
            // Arrange
            var controller = new CalculationController();

            // Act
            var result = controller.Sum(3, 5) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Value);
        }

        [Fact]
        public void Ping_ShouldReturnOkWithResponse()
        {
            // Arrange
            var controller = new PingController();

            // Act
            var result = controller.Ping() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Ping received.", result.Value);
        }
    }
}
