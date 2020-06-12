using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;
using System;

namespace Gear.ProjectManagement.Manager.Background.DeleteOutdatedNotifications
{
    public class DeleteOutdatedNotificationsHostedService : HostedServiceBase<DeleteOutdatedNotificationsHostedService, DeleteOutdatedNotificationsScopedService>
    {
        public DeleteOutdatedNotificationsHostedService(ILogger<DeleteOutdatedNotificationsHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }
    }
}
