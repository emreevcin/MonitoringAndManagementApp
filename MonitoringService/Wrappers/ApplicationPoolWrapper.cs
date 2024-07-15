using Microsoft.Web.Administration;
using MonitoringService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringService.Wrappers
{
    public class ApplicationPoolWrapper : IApplicationPoolWrapper
    {
        private readonly ApplicationPool _applicationPool;

        public ApplicationPoolWrapper(ApplicationPool applicationPool)
        {
            _applicationPool = applicationPool;
        }

        public ObjectState State => _applicationPool.State;

        public void Start()
        {
            _applicationPool.Start();
        }
    }
}
