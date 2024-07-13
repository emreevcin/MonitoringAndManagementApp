using System;
using Serilog;
using Util.Generics;

namespace Util
{
    public static class SerilogHelper
    {
        private static readonly ILogger logger;

        static SerilogHelper()
        {
            logger = LoggerUtil.ConfigureLogger(LoggerConfigurationType.Util);
        }

        public static ILogger GetLogger()
        {
            return logger;
        }
    }
}
