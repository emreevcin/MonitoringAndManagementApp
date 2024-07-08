using System;
using System.Configuration;
using System.Windows.Forms;
using Util;

namespace SettingsApplication
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            comboBoxService.Items.AddRange(new object[] { "FileWatcherService", "LogoMockWebApi" });
            comboBoxService.SelectedIndex = 0;

            LoadSettings(comboBoxService.Text);

            comboBoxService.SelectedIndexChanged += ComboBoxService_SelectedIndexChanged;
            btnFolderPath.Click += BtnFolderPath_Click;
        }

        private void ComboBoxService_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSettings(comboBoxService.Text);
        }

        private void LoadSettings(string serviceName)
        {
            var settings = JsonHelper.LoadServiceSettings(serviceName);

            if (settings != null)//null olma durumunda log
            {
                textBoxTimeInterval.Text = settings.MonitorInterval?.ToString();
                textBoxNumberOfRuns.Text = settings.NumberOfRuns?.ToString();
                comboBoxLogLevel.SelectedItem = settings.LogLevel?.ToString();

                if (serviceName.Contains("Service"))//methodlara bölünebilir.
                {
                    labelFolderPath.Visible = true;
                    textBoxFolderPath.Visible = true;
                    btnFolderPath.Visible = true;
                    labelUrl.Visible = false;
                    textBoxUrl.Visible = false;

                    textBoxFolderPath.Text = settings.FolderPath?.ToString();
                }
                else if (serviceName.Contains("WebApi"))
                {
                    labelFolderPath.Visible = serviceName.Contains("Service");
                    textBoxFolderPath.Visible = false;
                    btnFolderPath.Visible = false;
                    labelUrl.Visible = serviceName.Contains("WebApi");
                    textBoxUrl.Visible = true;

                    textBoxUrl.Text = settings.Url?.ToString();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var serviceName = comboBoxService.Text;

            if (string.IsNullOrEmpty(serviceName)) // null or empty
            {
                MessageBox.Show("Please select a service name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var serviceSettings = new ServiceSettings
            {
                ServiceName = serviceName,
                MonitorInterval = int.TryParse(textBoxTimeInterval.Text, out var interval) ? interval : 1,
                NumberOfRuns = int.TryParse(textBoxNumberOfRuns.Text, out var runs) ? runs : 1,
                LogLevel = comboBoxLogLevel.SelectedItem?.ToString()
            };

            if (serviceName.Contains("Service"))
            {
                serviceSettings.FolderPath = textBoxFolderPath.Text;
            }
            else if (serviceName.Contains("WebApi"))
            {
                serviceSettings.Url = textBoxUrl.Text;
            }

            try
            {
                JsonHelper.SaveServiceSettings(serviceName, serviceSettings);

                if (serviceName.Contains("Service"))
                {
                    string appConfigPath = $@"C:\Users\Emre.Evcin\source\repos\ManagementService\{serviceName}\App.config";
                    UpdateAppConfigLogLevel(serviceName, serviceSettings.LogLevel, serviceSettings.FolderPath, appConfigPath);
                }

                MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void BtnFolderPath_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxFolderPath.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void UpdateAppConfigLogLevel(string serviceName, string logLevel, string folderPath, string appConfigPath)
        {
            try
            {
                var map = new ExeConfigurationFileMap { ExeConfigFilename = appConfigPath };
                var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

                string logLevelKey = "serilog:minimum-level";
                string pathKey = "Path";

                if (config.AppSettings.Settings[logLevelKey] != null)
                {
                    config.AppSettings.Settings[logLevelKey].Value = logLevel;
                }
                else
                {
                    config.AppSettings.Settings.Add(logLevelKey, logLevel);
                }

                if (config.AppSettings.Settings[pathKey] != null)
                {
                    config.AppSettings.Settings[pathKey].Value = folderPath;
                }
                else
                {
                    config.AppSettings.Settings.Add(pathKey, folderPath);
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");//const
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating App.config: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
