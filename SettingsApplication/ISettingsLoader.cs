using Util;

namespace SettingsApplication
{
    public interface ISettingsLoader
    {
        ServiceSettingsDto LoadSettings(string serviceName);
    }
}
