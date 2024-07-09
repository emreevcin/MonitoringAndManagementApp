using System.Collections.Generic;
using Util; 

namespace MonitoringService
{
    public interface IServiceSettingsLoader
    {
        Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadServiceSettings();
    }
}
