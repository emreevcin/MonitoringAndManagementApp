using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringService.Interfaces
{
    public interface IApplicationPoolWrapper
    {
        ObjectState State { get; }
        void Start();
    }
}
