using Util;
using Serilog;

namespace LogoMockWebApi
{
    public class LogHelper
    {
        private static IConfiguration Configuration { get; set; }

        private readonly static string _serviceName = "LogoMockWebApi";

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigureLogger();
        }

        public static void ConfigureLogger()
        {
            var logLevel = LoggerUtil.GetLogLevel(_serviceName);

            var loggerConfig = LoggerUtil.ConfigureLogger("WebApi", logLevel, Configuration);

            Log.Logger = loggerConfig;
        }
    }
}