using Serilog;

namespace Util
{
    public static class SerilogHelper
    {
        private static readonly ILogger logger;

        static SerilogHelper()
        {
            logger = LoggerUtil.ConfigureLogger("Util");
        }

        public static ILogger GetLogger()
        {
            return logger;
        }
    }
}
