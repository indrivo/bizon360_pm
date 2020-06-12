using System.Collections.Generic;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Gear.Common.Extensions.DateTimeExtensions;
using System;
using Gear.Domain.Infrastructure;
using Microsoft.Extensions.Options;

namespace Gear.ProjectManagement.Manager.Background.SendDaylyNotifications
{
    public class SendDaylyNotificationsScopedService : IScopeBase<SendDaylyNotificationsScopedService>
    {
        private readonly IMediator _mediator;
        private readonly IEventService _eventService;
        private readonly IHostingEnvironment _environment;

        public SendDaylyNotificationsScopedService(IMediator mediator, IEventService eventService, IHostingEnvironment environment,
            IOptions<IndrivoNotificationOptions> options)
        {
            _mediator = mediator;
            _eventService = eventService;
            _environment = environment;
        }

        public async Task ExecuteProcess()
        {
            if (!DateTime.Now.IsWeekend() && _environment.IsProduction())
            {
                await _eventService.AddRange(new List<string> { "DailyLogsNotifications" });
                await _mediator.Publish(new YesterdayLogsNotificationsAdm(), CancellationToken.None);
                await _mediator.Publish(new YesterdayLogsNotificationsPMs(), CancellationToken.None);
            }
        }
    }
}
