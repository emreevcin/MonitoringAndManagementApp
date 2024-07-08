using Util;

namespace SettingsApplication
{
    public interface ISettingsLoader
    {
        ServiceSettings LoadSettings(string serviceName);
    }
}
