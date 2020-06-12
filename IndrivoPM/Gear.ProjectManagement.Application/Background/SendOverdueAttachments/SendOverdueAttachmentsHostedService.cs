using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.SendOverdueAttachments
{
    public class SendOverdueAttachmentsHostedService : HostedServiceBase<SendOverdueAttachmentsHostedService, SendOverdueAttachmentsScopedService>
    {
        public SendOverdueAttachmentsHostedService(ILogger<SendOverdueAttachmentsHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"{this.GetType().Name} service is starting.");

            _timer = new Timer(x => CreateScope(), null, TimeSpan.Zero, TimeSpan.FromHours(2));

            return Task.CompletedTask;
        }
    }
}
