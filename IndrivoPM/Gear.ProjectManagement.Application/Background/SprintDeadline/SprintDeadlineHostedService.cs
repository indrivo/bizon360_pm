using System;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.SprintDeadline
{
    public class SprintDeadlineHostedService : HostedServiceBase<SprintDeadlineHostedService, SprintDeadlineScopedService>
    {
        public SprintDeadlineHostedService(ILogger<SprintDeadlineHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }
    }
}
