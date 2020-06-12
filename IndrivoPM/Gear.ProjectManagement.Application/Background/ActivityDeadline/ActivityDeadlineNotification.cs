using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.ActivityDeadline
{
    public class ActivityDeadlineNotification : MediatorNotification
    {
        public static string EventName { get; } = "Notify activity deadline is one week, two weeks away or overdue";

        public static string GroupName { get; } = "Activity_ActivityReport";

        public static string PlatformName { get; } = "PM";

        public string Body { get; set; }
        public bool RedirectToActivity { get; set; } = false;

        public class ActivityDeadlineNotificationHandler : INotificationHandler<ActivityDeadlineNotification>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public ActivityDeadlineNotificationHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ActivityDeadlineNotification notification, CancellationToken cancellationToken)
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
                    Controller = "Activities",
                    Action = ""
                };

                if (notification.RedirectToActivity)
                {
                    message.MessageRedirectAction.Action = "Details";
                    message.MessageRedirectAction.Id = notification.PrimaryEntityId;
                }
                else
                {
                    message.MessageRedirectAction.Id = notification.GroupEntityId;
                }

                await _notification.SendAsync(message);
            }
        }
    }
}
