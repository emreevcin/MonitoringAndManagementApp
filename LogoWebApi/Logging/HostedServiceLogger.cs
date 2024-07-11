using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogoWebApi.Logging
{
    public class HostedServiceLogger : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Host started.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("Host stopped.");

            return Task.CompletedTask;
        }
    }
}
