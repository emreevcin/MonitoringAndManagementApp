using Serilog.Events;
using Serilog;

namespace LogoMockWebApi
{
    public class LogHelper
    {
        private static IConfiguration Configuration { get; set; }

        private static string serviceName = "LogoMockWebApi";

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigureLogger();
        }

        public static void ConfigureLogger()
        {
            string logLevelString = Util.LogLevel.GetLogLevel(serviceName);
            LogEventLevel logLevel = Util.LogLevel.ConvertToLogEventLevel(logLevelString);

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Is(logLevel)
                .Enrich.FromLogContext();

            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}
