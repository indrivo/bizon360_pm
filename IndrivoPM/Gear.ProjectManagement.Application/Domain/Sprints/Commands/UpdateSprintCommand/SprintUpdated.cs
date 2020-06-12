using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintCommand
{
    public class SprintUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any sprint is updated.";

        public static string GroupName { get; } = "Sprint_Sprint";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }

        public class SprintUpdatedHandler : INotificationHandler<SprintUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public SprintUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(SprintUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Sprint \"{notification.PrimaryEntityName}\" updated with changes: " + notification.Changes;
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
