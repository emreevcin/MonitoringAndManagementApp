using Newtonsoft.Json.Linq;

namespace Util
{
    public interface ISettingsRepository
    {
        JObject LoadAllSettings();
        void SaveAllSettings(dynamic allSettings);
    }
}
