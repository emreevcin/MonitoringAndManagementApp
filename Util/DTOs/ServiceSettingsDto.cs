﻿using Serilog.Events;
using Util.Generics;

namespace Util
{
    public class ServiceSettingsDto
    {
        public ServiceSettingsDto(string serviceName)
        {
            ServiceName = serviceName;
            MonitorInterval = Constants.DefaultMonitorInterval;
            NumberOfRuns = Constants.DefaultNumberOfRuns;
            LogLevel = Constants.DefaultLogLevel;
            Url = SettingsHelper.GetServiceType(serviceName) == ServiceTypes.WebApi ? Constants.DefaultWebApiUrl : null;
            FolderPath = SettingsHelper.GetServiceType(serviceName) == ServiceTypes.Service ? $"{Constants.DefaultFolderPath}" : null;
        }

        public ServiceSettingsDto(string serviceName, int monitorInterval, int numberOfRuns, LogEventLevel logLevel, string url, string folderPath)
        {
            ServiceName = serviceName;
            MonitorInterval = monitorInterval;
            NumberOfRuns = numberOfRuns;
            LogLevel = logLevel;
            Url = url;
            FolderPath = folderPath;
        }

        public ServiceSettingsDto()
        {
        }

        public string ServiceName { get; set; }
        public int MonitorInterval { get; set; }
        public int NumberOfRuns { get; set; }
        public LogEventLevel LogLevel { get; set; }
        public string Url { get; set; }
        public string FolderPath { get; set; }
    }
}
