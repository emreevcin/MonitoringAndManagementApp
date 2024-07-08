using Util;

namespace MonitoringService
{
    public interface IServiceMonitor
    {
        void MonitorService(string serviceName, ServiceSettings settings);
    }
}
