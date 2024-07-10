using System;
using System.Windows.Forms;
using Util;

namespace SettingsApplication
{
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

            var logCatcher = LoggerUtil.ConfigureLogger("App.Config");

            // var logCatcher = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

            Application.Run(new SettingsForm(logCatcher));
        }
    }
}
