using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateTrackerType
{
    public class TrackerTypeUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity subtype is updated.";

        public static string GroupName { get; } = "Administration_Subtype";

        public static string PlatformName { get; } = "ADM";

        public string Changes { get; set; }

        public class TrackerTypeUpdatedHandler : INotificationHandler<TrackerTypeUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public TrackerTypeUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(TrackerTypeUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();
                
                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Subtype \"{notification.PrimaryEntityName}\" updated with changes: " + notification.Changes;
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
