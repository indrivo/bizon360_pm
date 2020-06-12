using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.CreateActivityList
{
    public class ActivityListCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any activity list is created.";

        public static string GroupName { get; } = "Activity_ActivityList";

        public static string PlatformName { get; } = "PM";

        public class ActivityListCreatedHandler : INotificationHandler<ActivityListCreated>
        {
            private readonly IGearContext _context;

            private readonly INotificationService _notification;

            public ActivityListCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on activity list create
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(ActivityListCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Activity list \"{notification.PrimaryEntityName}\" created.";
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
