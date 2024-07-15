using MonitoringService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace MonitoringService.Wrappers
{
    public class SettingsHelperWrapper : ISettingsHelper
    {
        public Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadAllServiceSettings()
        {
            return SettingsHelper.LoadAllServiceSettings();
        }
    }
}
