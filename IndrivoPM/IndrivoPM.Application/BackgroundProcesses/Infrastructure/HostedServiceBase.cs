using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gear.Manager.Core.BackgroundProcesses.Infrastructure
{
    public class HostedServiceBase<THostedService, TScope> : IHostedService, IDisposable
    {
        protected readonly ILogger _logger;
        private IServiceProvider Services { get; }
        protected Timer _timer;


        public HostedServiceBase(ILogger<THostedService> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(THostedService)} service is starting.");

            _timer = new Timer(x => CreateScope(), null, TimeSpan.Zero, TimeSpan.FromHours(23));

            return Task.CompletedTask;
        }

        protected async Task CreateScope()
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopeBase<TScope>>();

                await scopedProcessingService.ExecuteProcess();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(THostedService)} is gracefully stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
