using System;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.SprintExpirationCheck
{
    public class SprintExpirationHostedService : HostedServiceBase<SprintExpirationHostedService, SprintExpirationScopedService>
    {
        public SprintExpirationHostedService(ILogger<SprintExpirationHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }
    }
}
