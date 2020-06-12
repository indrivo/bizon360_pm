using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerProject
{
    public class LoggedTimePerProjectHostedService : HostedServiceBase<LoggedTimePerProjectHostedService, LoggedTimePerProjectScopedService>
    {
        public LoggedTimePerProjectHostedService(ILogger<LoggedTimePerProjectHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{nameof(LoggedTimePerProjectHostedService)} service is starting.");
            
            _timer = new Timer(async x => 
            await RunAtTime(), null, TimeSpan.Zero, TimeSpan.FromHours(1));


            return Task.CompletedTask;
        }

        private async Task RunAtTime()
        {
            if (DateTime.Now.Hour == 19)
            {
                await CreateScope();
            }
        }
    }
}
