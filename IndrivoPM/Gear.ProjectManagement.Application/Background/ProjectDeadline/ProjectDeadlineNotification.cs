using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.ProjectDeadline
{
    public class ProjectDeadlineNotification : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any project deadline is two weeks, one month away or overdue.";

        public static string GroupName { get; } = "Project_ProjectReport";

        public static string PlatformName { get; } = "PM";

        public string Body { get; set; }

        public class ProjectDeadlineNotificationHandler : INotificationHandler<ProjectDeadlineNotification>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public ProjectDeadlineNotificationHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ProjectDeadlineNotification notification, CancellationToken cancellationToken)
            {
                if (!notification.Recipients.Any())
                {
                    await notification.GetUsersFromProfilesAsync(_context, _notification);
                }

                var message = notification.CreateNotificationMessage();
                message.Subject = "Reminder";
                message.Body = notification.Body;
                message.MessageRedirectAction = new MessageRedirectAction
                {
                    Controller = "Projects",
                    Action = "Details",
                    Id = notification.GroupEntityId
                };

                await _notification.SendAsync(message);
            }
        }
    }
}
