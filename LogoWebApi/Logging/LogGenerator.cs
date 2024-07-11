using Serilog;
using Util.Generics;
using Util;

namespace LogoWebApi.Logging
{
    public class LogGenerator
    {
        private static IConfiguration Configuration { get; set; }

        private readonly static string _serviceName = Enum.GetName(typeof(ServiceNames), ServiceNames.LogoWebApi);

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigureLogger();
        }

        public static void ConfigureLogger()
        {

            var serviceSettings = SettingsHelper.LoadServiceSettings(_serviceName);
            var logLevel = serviceSettings.LogLevel;
            var url = serviceSettings.Url;

            Log.Logger = LoggerUtil.ConfigureLogger(LoggerConfigurationType.WebApi, logLevel, Configuration);

            Log.Information($"Logger configured for {_serviceName} at {url} with log level {logLevel}");
        }
    }
}
