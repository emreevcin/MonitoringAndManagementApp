using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace SettingsApplication
{
    internal class JsonSettingsSaver : ISettingsSaver
    {
        public void SaveSettings(string serviceName, ServiceSettings settings)
        {
            JsonHelper.SaveServiceSettings(serviceName, settings);
        }
    }
}
