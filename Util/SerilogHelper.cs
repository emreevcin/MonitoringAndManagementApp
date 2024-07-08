using Microsoft.Extensions.Configuration;
using Serilog;


namespace Util
{
    public static class SerilogHelper
    {
        private static readonly ILogger logger;

        static SerilogHelper()
        {
            logger = new LoggerConfiguration()
                .WriteTo.File("C:\\MonitoringAndManagementApplication\\Logs\\MonitoringService-.log", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Warning()
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public static ILogger GetLogger()
        {
            return logger;
        }
    }
}
