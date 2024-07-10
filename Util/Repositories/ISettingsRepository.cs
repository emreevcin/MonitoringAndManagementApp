using System.Collections.Generic;

namespace Util
{
    public interface ISettingsRepository
    {
        Dictionary<string, Dictionary<string, ServiceSettingsDto>> LoadAllSettings();
        void SaveAllSettings(Dictionary<string, Dictionary<string, ServiceSettingsDto>> allSettings);
    }
}
