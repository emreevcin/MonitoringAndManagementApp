using Util;

namespace SettingsApplication
{
    internal class JsonSettingsSaver : ISettingsSaver
    {
        public void SaveSettings(string serviceName, ServiceSettingsDto settings)
        {
            SettingsJsonHelper.SaveServiceSettings(serviceName, settings);
        }
    }
}
