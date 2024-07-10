using Serilog.Events;

namespace Util.AppConfigSettings
{
    public interface IConfigUpdater
    {
        void UpdateAppConfigLogLevel(string serviceName, LogEventLevel logLevel, string appConfigPath);
    }
}
