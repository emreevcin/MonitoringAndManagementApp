using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using Util;


namespace LogoMockWebApi
{
    public class IISServiceWatcher : IHostedService, IDisposable
    {
        private Timer _timer;
        private bool _browserOpened;
        private string _url;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("IISServiceWatcher started.");
            _timer = new Timer(CheckIISAndOpenBrowser, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            _browserOpened = false;
            return Task.CompletedTask;
        }

        private void CheckIISAndOpenBrowser(object state)
        {
            _url = ServiceSettingManager.GetServiceUrl("LogoMockWebApi");
            if (IsIISRunning())
            {
                if (!_browserOpened)
                {
                    Log.Information("IIS detected. Opening browser.");
                    OpenBrowser(_url);
                    _browserOpened = true;
                }
            }
            else
            {
                Log.Information("IIS not detected.");
                _browserOpened = false;
            }
        }

        private bool IsIISRunning()
        {
            var iisProcesses = Process.GetProcessesByName("w3wp");
            bool isRunning = iisProcesses.Any();

            if (isRunning)
            {
                Log.Debug("IIS (w3wp.exe) processes detected: {Processes}", string.Join(", ", iisProcesses.Select(p => p.Id)));
            }
            else
            {
                Log.Debug("No IIS (w3wp.exe) processes detected.");
            }

            return isRunning;
        }

        private void OpenBrowser(string url)
        {
            try
            {
                Log.Information("Opening browser with URL: {Url}", url);

                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred while opening the browser: {ex.Message}");
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("IISServiceWatcher stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Log.Information("IISServiceWatcher disposed.");
            _timer?.Dispose();
        }
    }
}
