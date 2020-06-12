using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityList
{
    public class ActivityListUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity list is updated.";

        public static string GroupName { get; } = "Activity_ActivityList";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }

        public class ActivityListUpdatedHandler : INotificationHandler<ActivityListUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ActivityListUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on activity list update
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(ActivityListUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Activity list \"{notification.PrimaryEntityName}\" was updated with changes : " + notification.Changes;
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
