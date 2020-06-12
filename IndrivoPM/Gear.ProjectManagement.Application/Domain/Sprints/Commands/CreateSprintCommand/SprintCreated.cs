using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.CreateSprintCommand
{
    public class SprintCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any sprint is created.";

        public static string GroupName { get; } = "Sprint_Sprint";

        public static string PlatformName { get; } = "PM";

        public class SprintCreatedHandler : INotificationHandler<SprintCreated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public SprintCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on sprint create
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(SprintCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Sprint \"{notification.PrimaryEntityName}\" was created.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.GroupEntityId,
                        Action = "Details",
                        Controller = "Projects"
                    };

                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
