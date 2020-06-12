using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateActivityType
{
    public class ActivityTypeUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity type is updated.";

        public static string GroupName { get; } = "Administration_ActivityType";

        public static string PlatformName { get; } = "ADM";

        public class ActivityTypeUpdatedHandler : INotificationHandler<ActivityTypeUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ActivityTypeUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(ActivityTypeUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.Body = $"Activity Type \"{notification.PrimaryEntityName}\" was updated.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Action = "Index",
                        Controller = "Services"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
