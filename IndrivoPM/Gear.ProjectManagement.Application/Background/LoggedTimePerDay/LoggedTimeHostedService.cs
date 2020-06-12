using System;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerDay
{
    public class LoggedTimeHostedService : HostedServiceBase<LoggedTimeHostedService, LoggedTimeScopedService>
    {
        public LoggedTimeHostedService(ILogger<LoggedTimeHostedService> logger, IServiceProvider services) : base(logger, services)
        {
        }
    }
}
