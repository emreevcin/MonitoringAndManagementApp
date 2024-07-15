using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace MonitoringService.Interfaces
{
    public interface ISettingsHelper
    {
        Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadAllServiceSettings();
    }
}
