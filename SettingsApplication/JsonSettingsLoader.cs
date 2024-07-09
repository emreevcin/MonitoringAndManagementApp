using Util;

namespace SettingsApplication
{
    internal class JsonSettingsLoader : ISettingsLoader
    {
        public ServiceSettingsDto LoadSettings(string serviceName)
        {
            return SettingsJsonHelper.LoadServiceSettings(serviceName);
        }
    }
}
