using Serilog;
using System;
using System.Windows.Forms;

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

            var logCatcher = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            Application.Run(new SettingsForm(logCatcher));
        }
    }
}
