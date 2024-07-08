using Serilog.Events;

namespace Util
{
    public class ServiceSettingsFactory
    {
        public static ServiceSettings CreateDefaultServiceSettings(string serviceKey)
        {
            return new ServiceSettings
            {
                ServiceName = serviceKey,
                MonitorInterval = Constants.DefaultMonitorInterval,
                NumberOfRuns = Constants.DefaultNumberOfRuns,
                LogLevel = Constants.DefaultLogLevel,
                Url = serviceKey.Contains("WebApi") ? Constants.DefaultWebApiUrl : null,
                FolderPath = serviceKey.Contains("Service") ? $"{Constants.DefaultFolderPath}\\{serviceKey}" : null
            };
        }

        public static ServiceSettings CreateCustomServiceSettings(
            string serviceName,
            int monitorInterval = Constants.DefaultMonitorInterval,
            int numberOfRuns = Constants.DefaultNumberOfRuns,
            LogEventLevel logLevel = LogEventLevel.Information,
            string url = null,
            string folderPath = null)
        {
            return new ServiceSettings
            {
                ServiceName = serviceName,
                MonitorInterval = monitorInterval,
                NumberOfRuns = numberOfRuns,
                LogLevel = logLevel.ToString(),
                Url = url ?? (serviceName.Contains("WebApi") ? Constants.DefaultWebApiUrl : null),
                FolderPath = folderPath ?? (serviceName.Contains("Service") ? $"{Constants.DefaultFolderPath}\\{serviceName}" : null)
            };
        }
    }
}
