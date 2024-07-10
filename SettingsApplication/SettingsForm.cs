using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Windows.Forms;
using Util;
using Util.AppConfigSettings;

namespace SettingsApplication
{
    public partial class SettingsForm : Form
    {
        private readonly ILogger _logger;

        public SettingsForm(ILogger logCatcher)
        {
            _logger = logCatcher;

            InitializeComponent();

            comboBoxService.Items.AddRange(new object[] { "FileWatcherService", "LogoWebApi" });
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
                ServiceSettingsDto settings = SettingsHelper.LoadServiceSettings(serviceName);

                if (settings != null)
                {
                    textBoxTimeInterval.Text = settings.MonitorInterval.ToString();
                    textBoxNumberOfRuns.Text = settings.NumberOfRuns.ToString();
                    comboBoxLogLevel.SelectedItem = Enum.GetName(typeof(LogEventLevel), settings.LogLevel);

                    UpdateVisibility(serviceName);
                    UpdateFields(settings);
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

        private void SaveSettings(ServiceSettingsDto serviceSettings)
        {
            SettingsHelper.CheckServiceNameAndLogError(serviceSettings);

            string serviceName = serviceSettings.ServiceName;

            SettingsHelper.SaveServiceSettings(serviceName, serviceSettings);

            if (serviceName.Contains("Service"))
            {
                string appConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\{serviceName}\App.config");
                var updater = new AppConfigUpdater();
                updater.UpdateAppConfigLogLevel(serviceName, serviceSettings.LogLevel, appConfigPath);
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

        private void UpdateFields(ServiceSettingsDto settings)
        {
            SettingsHelper.CheckServiceNameAndLogError(settings);

            string serviceName = settings.ServiceName;

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
            LogEventLevel logLevel = Constants.DefaultLogLevel;
            Enum.TryParse(comboBoxLogLevel.SelectedItem?.ToString(), out logLevel);

            try
            {
                if (!ValidateInputs(monitorIntervalText, numberOfRunsText, serviceName))
                {
                    _logger.Error("Validation failed. Please correct the errors and try again.");
                    return;
                }

                var serviceSettings = CreateServiceSettings(serviceName, monitorIntervalText, numberOfRunsText, logLevel);
                SaveSettings(serviceSettings);

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

        private bool ValidateInputs(string monitorIntervalText, string numberOfRunsText, string serviceName)
        {
            if (!Validation.CheckValidation(monitorIntervalText, numberOfRunsText, serviceName, textBoxFolderPath, textBoxUrl, _logger))
            {
                return false;
            }

            if (!int.TryParse(monitorIntervalText, out _))
            {
                _logger.Error($"Monitor Interval should be a valid integer. {monitorIntervalText} is not valid.");
                return false;
            }

            if (!int.TryParse(numberOfRunsText, out _))
            {
                _logger.Error($"Number of Runs should be a valid integer. {numberOfRunsText} is not valid.");
                return false;
            }

            return true;
        }

        private ServiceSettingsDto CreateServiceSettings(string serviceName, string monitorIntervalText, string numberOfRunsText, LogEventLevel logLevel)
        {
            var monitorInterval = int.Parse(monitorIntervalText);
            var numberOfRuns = int.Parse(numberOfRunsText);

            var serviceSettings = new ServiceSettingsDto
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

            return serviceSettings;
        }
    }
}
