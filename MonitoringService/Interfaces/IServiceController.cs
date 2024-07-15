using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringService.Interfaces
{
    public interface IServiceController
    {
        ServiceControllerStatus Status { get; }
        void Start();
        void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout);
    }
}
