using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;


namespace LogoMockWebApi
{
    public class IISServiceWatcher : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckIISAndOpenBrowser, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }

        private void CheckIISAndOpenBrowser(object state)
        {
            if (IsIISRunning())
            {
                OpenBrowser("http://localhost:88/api/Ping/ping");
            }
        }

        private bool IsIISRunning()
        {
            return Process.GetProcessesByName("w3wp").Any();
        }

        private void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                Log.Error($"An error occurred while opening the browser: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}