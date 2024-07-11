using Util;

namespace MonitoringService
{
    public interface IServiceMonitor
    {
        void MonitorService(ServiceSettingsDto settings);
    }
}
