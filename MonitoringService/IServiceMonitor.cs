using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace MonitoringService
{
    public interface IServiceMonitor
    {
        void MonitorService(string serviceName, ServiceSettings settings);
    }
}
