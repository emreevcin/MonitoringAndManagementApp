using Serilog.Events;

namespace Util
{
    public static class Constants
    {
        public const string settingsFilePath = "C:\\MonitoringAndManagementApplication\\appsettings.json";
        public const string logFilePath = "C:\\MonitoringAndManagementApplication\\Logs\\MonitoringService-.log";
        public const LogEventLevel DefaultLogLevel = LogEventLevel.Information;
        public const int DefaultMonitorInterval = 5;
        public const int DefaultNumberOfRuns = 3;
        public const string DefaultFolderPath = "C:\\MonitoringAndManagementApplication\\Demo";
        public const string DefaultWebApiUrl = "http://localhost:121/swagger/index.html";
    }
}
