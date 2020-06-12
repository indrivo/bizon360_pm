using System;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.ActivityDeadline
{
    public class ActivityDeadlineHostedService : HostedServiceBase<ActivityDeadlineHostedService, ActivityDeadlineScopedService>
    {
        public ActivityDeadlineHostedService(ILogger<ActivityDeadlineHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }
    }
}