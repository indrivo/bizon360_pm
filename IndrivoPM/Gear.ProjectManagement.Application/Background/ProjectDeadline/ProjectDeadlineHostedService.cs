using System;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.ProjectDeadline
{
    public class ProjectDeadlineHostedService : HostedServiceBase<ProjectDeadlineHostedService, ProjectDeadlineScopedService>
    {
        public ProjectDeadlineHostedService(ILogger<ProjectDeadlineHostedService> logger,
            IServiceProvider services) : base(logger, services)
        {
        }
    }
}
