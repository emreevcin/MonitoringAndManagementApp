using Util;

namespace SettingsApplication
{
    internal class JsonSettingsLoader : ISettingsLoader
    {
        public ServiceSettings LoadSettings(string serviceName)
        {
            return JsonHelper.LoadServiceSettings(serviceName);
        }
    }
}
