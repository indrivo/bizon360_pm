using System;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.PlanedTasks
{
    public class PlanedTasksHostedService : HostedServiceBase<PlanedTasksHostedService, PlanedTasksScopedService>
    {
        public PlanedTasksHostedService(ILogger<PlanedTasksHostedService> logger, IServiceProvider services) 
            : base(logger, services)
        {
        }
    }
}
