using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Serilog;
using Util;

namespace MaMApp.Test.Services.Tests
{
    public class FileWatcherServiceTests
    {
        private readonly Mock<ILogger> _mockLogger;

        public FileWatcherServiceTests()
        {
            _mockLogger = new Mock<ILogger>();
        }

        [Fact]
        public void OnStart_ShouldConfigureFileSystemWatcher()
        {
            // Arrange
            string testPath = Constants.validPath;
            var fileWatcherService = new FileWatcherService.FileWatcherService(_mockLogger.Object, testPath);

            // Act
            fileWatcherService.StartService(null);

            // Assert
            Assert.NotNull(fileWatcherService._watcher);
            Assert.True(fileWatcherService._watcher.EnableRaisingEvents);
        }

        [Fact]
        public void OnFileSystemEvent_ShouldLogInformation()
        {
            // Arrange
            string testPath = Constants.validPath;
            var fileWatcherService = new FileWatcherService.FileWatcherService(_mockLogger.Object, testPath);
            var eventArgs = new FileSystemEventArgs(WatcherChangeTypes.Changed, testPath, Constants.testFileName);

            // Act
            fileWatcherService.OnFileSystemEvent(null, eventArgs);

            // Assert
            _mockLogger.Verify(l => l.Information($"{eventArgs.ChangeType}: {eventArgs.Name}"), Times.Once);
        }

        [Fact]
        public void OnStart_ShouldLogError_WhenPathDoesNotExist()
        {
            // Arrange
            string invalidPath = Constants.invalidPath;
            var fileWatcherService = new FileWatcherService.FileWatcherService(_mockLogger.Object, invalidPath);

            // Act
            fileWatcherService.StartService(null);

            // Assert
            _mockLogger.Verify(l => l.Error($"Path {invalidPath} does not exist."), Times.Once);
        }

        [Fact]
        public void OnStart_ShouldLogInformation()
        {
            // Arrange
            string validPath = Constants.validPath;
            var fileWatcherService = new FileWatcherService.FileWatcherService(_mockLogger.Object, validPath);

            // Act
            fileWatcherService.StartService(null);

            // Assert
            _mockLogger.Verify(l => l.Information("Service started."), Times.Once);
        }

        [Fact]
        public void OnStop_ShouldLogInformation()
        {
            // Arrange
            string validPath = Constants.validPath;
            var fileWatcherService = new FileWatcherService.FileWatcherService(_mockLogger.Object, validPath);

            // Act
            fileWatcherService.StopService();

            // Assert
            _mockLogger.Verify(l => l.Information("Service stopped."), Times.Once);
        }
    }
}
