using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerProject
{
    public class LoggedTimePerProjectNotification : MediatorNotification
    {
        public static string EventName { get; } = "Notify daily, weekly, monthly on logged time / day per team members.";

        public static string GroupName { get; } = "Project_ProjectReport";

        public static string PlatformName { get; } = "PM";

        public string Body { get; set; }

        public class LoggedTimePerProjectNotificationHandler : INotificationHandler<LoggedTimePerProjectNotification>
        {
            private readonly INotificationService _notification;

            public LoggedTimePerProjectNotificationHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(LoggedTimePerProjectNotification notification, CancellationToken cancellationToken)
            {
                var message = notification.CreateNotificationMessage();
                message.Subject = "Reminder";
                message.Body = notification.Body;
                message.MessageRedirectAction = new MessageRedirectAction
                {
                    Controller = "Projects",
                    Action = "LoggedTime",
                    Id = notification.GroupEntityId
                };

                await _notification.SendAsync(message);
            }
        }
    }
}
