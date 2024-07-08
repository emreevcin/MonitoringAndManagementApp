using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsApplication
{
    public interface IConfigUpdater
    {
        void UpdateAppConfigLogLevel(string serviceName, string logLevel, string appConfigPath);
    }
}
