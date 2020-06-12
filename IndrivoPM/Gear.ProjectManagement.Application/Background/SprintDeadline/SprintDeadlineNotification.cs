using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.SprintDeadline
{
    public class SprintDeadlineNotification : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any sprint deadline is one week, two weeks or overdue.";

        public static string GroupName { get; } = "Sprint_SprintReport";

        public static string PlatformName { get; } = "ADM";

        public string Body { get; set; }

        public class SprintDeadlineNotificationHandler : INotificationHandler<SprintDeadlineNotification>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public SprintDeadlineNotificationHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(SprintDeadlineNotification notification, CancellationToken cancellationToken)
            {
                var message = notification.CreateNotificationMessage();

                if (!notification.Recipients.Any())
                {
                    await notification.GetUsersFromProfilesAsync(_context, _notification);
                }

                message.Subject = "Reminder";
                message.Body = notification.Body;
                message.MessageRedirectAction = new MessageRedirectAction
                {
                    Controller = "Activities",
                    Action = "",
                    Id = notification.GroupEntityId,
                    AdditionalData = "?view=Sprint"
                };
                
                await _notification.SendAsync(message);
            }
        }
    }
}
