using Serilog;
using System;
using System.Windows.Forms;

namespace SettingsApplication
{
    public static class Validation
    {
        public static bool ValidateUrl(string url)
        {
            Uri resultUri;
            return Uri.TryCreate(url, UriKind.Absolute, out resultUri);
        }

        public static bool ValidateFolderPath(string path)
        {
            try
            {
                System.IO.Path.GetFullPath(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidateInteger(string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        public static bool CheckValidation(string monitorInterval, string numberOfRuns, string serviceName, TextBox textBoxFolderPath, TextBox textBoxUrl, ILogger logger)
        {
            bool isValid = true;

            if (!ValidateInteger(monitorInterval))
            {
                logger.Error("Monitor Interval should be a valid integer.");
                MessageBox.Show("Please enter a valid Monitor Interval.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }

            if (!ValidateInteger(numberOfRuns))
            {
                logger.Error("Number of Runs should be a valid integer.");
                MessageBox.Show("Please enter a valid Number of Runs.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }

            if (serviceName.Contains("Service") && !ValidateFolderPath(textBoxFolderPath.Text))
            {
                logger.Error("Invalid folder path: {Path}", textBoxFolderPath.Text);
                MessageBox.Show("Please enter a valid Folder Path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }

            if (serviceName.Contains("WebApi") && !ValidateUrl(textBoxUrl.Text))
            {
                logger.Error("Invalid URL format: {Url}", textBoxUrl.Text);
                MessageBox.Show("Please enter a valid URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }

            return isValid;
        }
    }
}
