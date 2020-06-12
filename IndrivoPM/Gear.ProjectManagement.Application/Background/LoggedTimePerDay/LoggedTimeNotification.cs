using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerDay
{
    public class LoggedTimeNotification : MediatorNotification
    {
        public List<string> Emails { get; set; }

        public class LoggedTimeNotificationHandler : INotificationHandler<LoggedTimeNotification>
        {
            private readonly INotificationService _notification;

            public LoggedTimeNotificationHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(LoggedTimeNotification notification, CancellationToken cancellationToken)
            {
                foreach (var email in notification.Emails)
                {
                    var message = notification.CreateNotificationMessage();
                    message.Subject = "Daily Report on people who don't log their time";
                    message.Body = "You have filled less hours that you should today";
                    message.To = email;

                    await _notification.SendAsync(message);
                }

            }
        }
    }
}
