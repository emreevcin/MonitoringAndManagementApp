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
