using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using Serilog;
using Util;
using Xunit;

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
            var logInformationInvocations = _mockLogger.Invocations
                .Count(inv => inv.Method.Name == nameof(_mockLogger.Object.Information) &&
                              inv.Arguments.Contains($"{eventArgs.ChangeType}: {eventArgs.Name}"));

            Assert.Equal(1, logInformationInvocations);
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
            var logErrorInvocations = _mockLogger.Invocations
                .Count(inv => inv.Method.Name == nameof(_mockLogger.Object.Error) &&
                              inv.Arguments.Contains($"Path {invalidPath} does not exist."));

            Assert.Equal(1, logErrorInvocations);
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
            var logInformationInvocations = _mockLogger.Invocations
                .Count(inv => inv.Method.Name == nameof(_mockLogger.Object.Information) &&
                              inv.Arguments.Contains("Service started."));

            Assert.Equal(1, logInformationInvocations);
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
            var logInformationInvocations = _mockLogger.Invocations
                .Count(inv => inv.Method.Name == nameof(_mockLogger.Object.Information) &&
                              inv.Arguments.Contains("Service stopped."));

            Assert.Equal(1, logInformationInvocations);
        }
    }
}
