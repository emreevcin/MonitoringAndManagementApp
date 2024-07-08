using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var settingsLoader = new JsonSettingsLoader();
            var settingsSaver = new JsonSettingsSaver();
            var configUpdater = new AppConfigUpdater();

            Application.Run(new SettingsForm(settingsLoader, settingsSaver, configUpdater));
        }
    }
}
