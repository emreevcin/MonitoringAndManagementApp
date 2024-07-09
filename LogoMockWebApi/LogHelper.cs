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
            var logLevel = Util.LogLevel.GetLogEventLevel(_serviceName);


            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Is(logLevel)
                .Enrich.FromLogContext();

            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}
