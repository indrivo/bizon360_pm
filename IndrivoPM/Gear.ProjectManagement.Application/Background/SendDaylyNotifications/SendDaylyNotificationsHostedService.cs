using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gear.ProjectManagement.Manager.Background.SendDaylyNotifications
{
    public class SendDaylyNotificationsHostedService : HostedServiceBase<SendDaylyNotificationsHostedService, SendDaylyNotificationsScopedService>
    {
        public SendDaylyNotificationsHostedService(ILogger<SendDaylyNotificationsHostedService> logger, IServiceProvider services) : base(logger, services)
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
