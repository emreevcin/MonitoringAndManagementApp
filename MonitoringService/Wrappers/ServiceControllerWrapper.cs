using MonitoringService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringService.Wrappers
{
    public class ServiceControllerWrapper : IServiceController
    {
        private readonly ServiceController _serviceController;

        public ServiceControllerWrapper(string serviceName)
        {
            _serviceController = new ServiceController(serviceName);
        }

        public ServiceControllerStatus Status => _serviceController.Status;

        public void Start()
        {
            _serviceController.Start();
        }


        public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
        {
            _serviceController.WaitForStatus(desiredStatus, timeout);
        }
    }
}
