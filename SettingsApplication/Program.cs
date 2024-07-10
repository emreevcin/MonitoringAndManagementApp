using System;
using System.Windows.Forms;
using Util;
using Util.Generics;

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

            var logCatcher = LoggerUtil.ConfigureLogger(LoggerConfigurationType.AppConfig);

            Application.Run(new SettingsForm(logCatcher));
        }
    }
}
