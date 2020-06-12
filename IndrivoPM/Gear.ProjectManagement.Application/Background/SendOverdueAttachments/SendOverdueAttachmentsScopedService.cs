using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Gear.Common.Extensions.DateTimeExtensions;

namespace Gear.ProjectManagement.Manager.Background.SendOverdueAttachments
{
    public class SendOverdueAttachmentsScopedService : IScopeBase<SendOverdueAttachmentsScopedService>
    {
        private readonly IMediator _mediator;

        private readonly IEventService _eventService;

        private readonly IHostingEnvironment _environment;

        public SendOverdueAttachmentsScopedService(IMediator mediator, IEventService eventService, IHostingEnvironment environment)
        {
            _mediator = mediator;
            _eventService = eventService;
            _environment = environment;
        }

        public async Task ExecuteProcess()
        {
            if (!DateTime.Now.IsWeekend() && _environment.IsProduction())
            {
                await _eventService.AddRange(new List<string> { "DailyOverdueNotifications" });
                await _mediator.Publish(new OverdueNotificationsForPMs(), CancellationToken.None);
                await _mediator.Publish(new OverdueNotificationsForAdm(), CancellationToken.None);
            }
        }
    }
}
