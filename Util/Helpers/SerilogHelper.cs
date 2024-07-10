using Serilog;


namespace Util
{
    public static class SerilogHelper
    {
        private static readonly ILogger logger;

        static SerilogHelper()
        {
            logger = new LoggerConfiguration()
                .WriteTo.File(Constants.logFilePath, rollingInterval: RollingInterval.Day)
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
