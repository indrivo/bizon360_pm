using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityListStatus
{
    public class ActivityListStatusUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity list status is updated.";

        public static string GroupName { get; } = "Activity_ActivityList";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }

        public class ActivityListStatusUpdatedHandler : INotificationHandler<ActivityListStatusUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ActivityListStatusUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on activity list status updated
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(ActivityListStatusUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Activity list status \"{notification.PrimaryEntityName}\" was updated with changes : " + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.GroupEntityId,
                        Action = "Details",
                        Controller = "Project"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
