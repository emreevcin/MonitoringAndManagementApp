namespace Util.AppConfigSettings
{
    public interface IConfigUpdater
    {
        void UpdateAppConfigLogLevel(string serviceName, string logLevel, string appConfigPath);
    }
}
