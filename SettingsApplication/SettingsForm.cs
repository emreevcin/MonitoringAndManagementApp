using System;
using System.Windows.Forms;
using Util;

namespace SettingsApplication
{
    public partial class SettingsForm : Form
    {
        private readonly ISettingsLoader _settingsLoader;
        private readonly ISettingsSaver _settingsSaver;
        private readonly IConfigUpdater _configUpdater;

        public SettingsForm(ISettingsLoader settingsLoader, ISettingsSaver settingsSaver, IConfigUpdater configUpdater)
        {
            _settingsLoader = settingsLoader;
            _settingsSaver = settingsSaver;
            _configUpdater = configUpdater;

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
            try
            {
                var jsonObject = JsonHelper.LoadServiceSettings(serviceName);

                if (jsonObject != null)
                {
                    var settings = jsonObject.ToObject<ServiceSettings>(); 

                    textBoxTimeInterval.Text = settings.MonitorInterval?.ToString();
                    textBoxNumberOfRuns.Text = settings.NumberOfRuns?.ToString();
                    comboBoxLogLevel.SelectedItem = settings.LogLevel;

                    UpdateVisibility(serviceName);
                    UpdateFields(serviceName, settings);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateVisibility(string serviceName)
        {
            bool isService = serviceName.Contains("Service");
            bool isWebApi = serviceName.Contains("WebApi");

            labelFolderPath.Visible = isService;
            textBoxFolderPath.Visible = isService;
            btnFolderPath.Visible = isService;
            labelUrl.Visible = isWebApi;
            textBoxUrl.Visible = isWebApi;
        }

        private void UpdateFields(string serviceName, ServiceSettings settings)
        {
            if (serviceName.Contains("Service"))
            {
                textBoxFolderPath.Text = settings.FolderPath?.ToString();
            }
            else if (serviceName.Contains("WebApi"))
            {
                textBoxUrl.Text = settings.Url?.ToString();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var serviceName = comboBoxService.Text;

            if (string.IsNullOrEmpty(serviceName))
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
                _settingsSaver.SaveSettings(serviceName, serviceSettings);

                if (serviceName.Contains("Service"))
                {
                    string appConfigPath = $@"C:\Users\Emre.Evcin\source\repos\ManagementService\{serviceName}\App.config";
                    _configUpdater.UpdateAppConfigLogLevel(serviceName, serviceSettings.LogLevel, serviceSettings.FolderPath, appConfigPath);
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
    }
}
