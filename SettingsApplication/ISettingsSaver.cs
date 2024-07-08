using Util;

namespace SettingsApplication
{
    public interface ISettingsSaver
    {
        void SaveSettings(string serviceName, ServiceSettings settings);
    }
}
