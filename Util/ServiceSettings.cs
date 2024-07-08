using Serilog.Events;

namespace Util
{
    public class ServiceSettings
    {
        public ServiceSettings()
        {
            LogLevel = "information";
        }
        public string ServiceName { get; set; }
        public int MonitorInterval { get; set; }
        public int NumberOfRuns { get; set; }
        public string LogLevel { get; set; }
        public string Url { get; set; }
        public string FolderPath { get; set; }
    }
}
