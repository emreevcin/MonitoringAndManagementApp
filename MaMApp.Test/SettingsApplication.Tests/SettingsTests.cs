using Moq;
using Serilog;
using Serilog.Events;
using System;
using Util;
using Xunit;

namespace MaMApp.Test.SettingsApplication.Tests
{
    public class SettingsTests
    {
        private readonly Mock<ISettingsRepository> _settingsRepositoryMock;

        public SettingsTests()
        {
            _settingsRepositoryMock = new Mock<ISettingsRepository>();
        }

        [Fact]
        public void LoadServiceSettings_ReturnsCorrectSettings()
        {
            // Arrange
            var serviceKey = "FileWatcherService";
            var expectedSettings = new ServiceSettingsDto(serviceKey)
            {
                MonitorInterval = 5,
                NumberOfRuns = 3,
                LogLevel = LogEventLevel.Information
            };
            var allSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
            {
                { "Services", new Dictionary<string, ServiceSettingsDto> { { serviceKey, expectedSettings } } }
            };

            _settingsRepositoryMock.Setup(repo => repo.LoadAllSettings()).Returns(allSettings);

            // Act
            var result = SettingsHelper.LoadServiceSettings(serviceKey);

            // Assert
            Assert.Equal(expectedSettings.MonitorInterval, result.MonitorInterval);
            Assert.Equal(expectedSettings.NumberOfRuns, result.NumberOfRuns);
            Assert.Equal(expectedSettings.LogLevel, result.LogLevel);
        }

        [Fact]
        public void LoadServiceSettings_ReturnsDefaultSettings_WhenServiceNotFound()
        {
            // Arrange
            var serviceKey = "NonExistentService";
            var allSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>();

            _settingsRepositoryMock.Setup(repo => repo.LoadAllSettings()).Returns(allSettings);

            // Act
            var result = SettingsHelper.LoadServiceSettings(serviceKey);

            // Assert
            Assert.Equal(serviceKey, result.ServiceName);
            Assert.Equal(Constants.DefaultMonitorInterval, result.MonitorInterval);
            Assert.Equal(Constants.DefaultNumberOfRuns, result.NumberOfRuns);
            Assert.Equal(Constants.DefaultLogLevel, result.LogLevel);
        }

        [Fact]
        public void LoadServiceSettings_ReturnsDefaultSettings_WhenAllSettingsAreNull()
        {
            // Arrange
            var serviceKey = "FileWatcherService";
            Dictionary<string, Dictionary<string, ServiceSettingsDto>> allSettings = null;

            _settingsRepositoryMock.Setup(repo => repo.LoadAllSettings()).Returns(allSettings);

            // Act
            var result = SettingsHelper.LoadServiceSettings(serviceKey);

            // Assert
            Assert.Equal(serviceKey, result.ServiceName);
            Assert.Equal(Constants.DefaultMonitorInterval, result.MonitorInterval);
            Assert.Equal(Constants.DefaultNumberOfRuns, result.NumberOfRuns);
            Assert.Equal(Constants.DefaultLogLevel, result.LogLevel);
        }

        [Fact]
        public void LoadAllServiceSettings_ReturnsAllSettings()
        {
            // Arrange
            var expectedServiceKey = "FileWatcherService";
            var expectedSettings = new ServiceSettingsDto(expectedServiceKey)
            {
                MonitorInterval = 5,
                NumberOfRuns = 3,
                LogLevel = LogEventLevel.Information,
                FolderPath = ".\\"
            };

            var allSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
                {
                    {
                        "Services", new Dictionary<string, ServiceSettingsDto>
                        {
                            { expectedServiceKey, expectedSettings }
                        }
                    }
                };

            // Set up mock repository to return allSettings
            _settingsRepositoryMock.Setup(repo => repo.LoadAllSettings()).Returns(allSettings);

            // Act
            var result = SettingsHelper.LoadAllServiceSettings();

            // Assert
            Assert.True(result.ContainsKey("Services"), "Expected 'Services' category in returned settings.");
            Assert.True(result["Services"].ContainsKey(expectedServiceKey), $"Expected '{expectedServiceKey}' to exist in 'Services' category.");

            var actualSettings = result["Services"][expectedServiceKey];
            Assert.Equal(expectedSettings.MonitorInterval, actualSettings.MonitorInterval);
            Assert.Equal(expectedSettings.NumberOfRuns, actualSettings.NumberOfRuns);
            Assert.Equal(expectedSettings.LogLevel, actualSettings.LogLevel);
            Assert.Equal(expectedSettings.FolderPath, actualSettings.FolderPath);
        }

        [Fact]
        public void SaveServiceSettings_SavesCorrectly()
        {
            // Arrange
            var serviceKey = "FileWatcherService";
            var newSettings = new ServiceSettingsDto(serviceKey)
            {
                MonitorInterval = 5,
                NumberOfRuns = 3,
                LogLevel = LogEventLevel.Information
            };

            var initialSettings = new Dictionary<string, Dictionary<string, ServiceSettingsDto>>
                {
                    {
                        "Services", new Dictionary<string, ServiceSettingsDto>
                        {
                            { serviceKey, new ServiceSettingsDto(serviceKey) }
                        }
                    }
                };

            // Mock setup
            _settingsRepositoryMock.Setup(repo => repo.LoadAllSettings()).Returns(initialSettings);

            // Act
            SettingsHelper.SaveServiceSettings(serviceKey, newSettings);

            // Assert
            _settingsRepositoryMock.Setup(repo => repo.SaveAllSettings(It.IsAny<Dictionary<string, Dictionary<string, ServiceSettingsDto>>>())).Verifiable();


            // Further assertions if needed
            var savedSettings = _settingsRepositoryMock.Object.LoadAllSettings();
            Assert.Equal(newSettings.MonitorInterval, savedSettings["Services"][serviceKey].MonitorInterval);
            Assert.Equal(newSettings.NumberOfRuns, savedSettings["Services"][serviceKey].NumberOfRuns);
            Assert.Equal(newSettings.LogLevel, savedSettings["Services"][serviceKey].LogLevel);
        }
    }
}
