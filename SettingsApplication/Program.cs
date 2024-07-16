using System;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;
using Util;
using Util.Generics;

namespace SettingsApplication
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var logCatcher = LoggerUtil.ConfigureLogger(LoggerConfigurationType.AppConfig);

            Application.Run(new SettingsForm(logCatcher));
        }
    }
}
