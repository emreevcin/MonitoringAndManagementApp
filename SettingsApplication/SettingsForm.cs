using Serilog;
using System;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using Util;

namespace SettingsApplication
{
    public partial class SettingsForm : Form
    {
        private readonly ILogger _logger;
        private readonly ISettingsLoader _settingsLoader;
        private readonly ISettingsSaver _settingsSaver;
        private readonly IConfigUpdater _configUpdater;

        public SettingsForm(ILogger logCatcher, ISettingsLoader settingsLoader, ISettingsSaver settingsSaver, IConfigUpdater configUpdater)
        {
            _logger = logCatcher;
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
                ServiceSettings settings = JsonHelper.LoadServiceSettings(serviceName);

                if (settings != null)
                {
                    textBoxTimeInterval.Text = settings.MonitorInterval.ToString();
                    textBoxNumberOfRuns.Text = settings.NumberOfRuns.ToString();
                    comboBoxLogLevel.SelectedItem = settings.LogLevel;

                    UpdateVisibility(serviceName);
                    UpdateFields(serviceName, settings);
                }
                else
                {
                    _logger.Warning("Settings file not found for {ServiceName}.", serviceName);
                    MessageBox.Show("Settings file not found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while loading settings.");
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
                _logger.Error("Please select a service name. Service name cannot be null or empty.");
                MessageBox.Show("Please select a service name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var monitorIntervalText = textBoxTimeInterval.Text;
            var numberOfRunsText = textBoxNumberOfRuns.Text;
            var logLevel = comboBoxLogLevel.SelectedItem?.ToString();

            try
            {
                if (!Validation.CheckValidation(monitorIntervalText, numberOfRunsText, serviceName, textBoxFolderPath, textBoxUrl, _logger))
                {
                    _logger.Error("Validation failed. Please correct the errors and try again.");
                    return;
                }

                if (!int.TryParse(monitorIntervalText, out var monitorInterval))
                {
                    _logger.Error($"Monitor Interval should be a valid integer. {monitorIntervalText} is not valid.");
                    throw new FormatException("Monitor Interval should be a valid integer.");
                }

                if (!int.TryParse(numberOfRunsText, out var numberOfRuns))
                {
                    _logger.Error($"Number of Runs should be a valid integer. {numberOfRunsText} is not valid.");
                    throw new FormatException("Number of Runs should be a valid integer.");
                }

                var serviceSettings = new ServiceSettings
                {
                    ServiceName = serviceName,
                    MonitorInterval = monitorInterval,
                    NumberOfRuns = numberOfRuns,
                    LogLevel = logLevel
                };

                if (serviceName.Contains("Service"))
                {
                    serviceSettings.FolderPath = textBoxFolderPath.Text;
                }
                else if (serviceName.Contains("WebApi"))
                {
                    serviceSettings.Url = textBoxUrl.Text;
                }

                _settingsSaver.SaveSettings(serviceName, serviceSettings);

                if (serviceName.Contains("Service"))
                {
                    string appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\{serviceName}\App.config");
                    _configUpdater.UpdateAppConfigLogLevel(serviceName, serviceSettings.LogLevel, appConfigPath);
                }
                _logger.Information("Settings saved successfully!");
                MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException ex)
            {
                _logger.Error(ex, "Invalid input format: {Message}", ex.Message);
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while saving settings.");
                MessageBox.Show($"An error occurred while saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
               LoadSettings(serviceName);
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
