using MonitoringService;
using Moq;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Microsoft.Web.Administration;
using Util;
using Xunit;
using MonitoringService.Helpers;

namespace MaMApp.Test
{
    public class MonitoringServiceTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IServiceMonitor> _windowsServiceMonitorMock;
        private readonly Mock<IServiceMonitor> _iisServiceMonitorMock;

        public MonitoringServiceTests()
        {
            _loggerMock = new Mock<ILogger>();
            _windowsServiceMonitorMock = new Mock<IServiceMonitor>();
            _iisServiceMonitorMock = new Mock<IServiceMonitor>();
        }

        [Fact]
        public void CheckAndRestartAppPool_AppPoolNull_LogsWarning()
        {
            // Arrange
            var appPool = (ApplicationPool)null;
            var settings = new ServiceSettingsDto("TestAppPool");
            var loggerMock = new Mock<ILogger>();

            // Act
            ServiceHelpers.CheckAndRestartAppPool(appPool, settings, loggerMock.Object);

            // Assert
            loggerMock.Verify(
                logger => logger.Warning("Application pool 'TestAppPool' not found."),
                Times.Once);
        }
    }
}
