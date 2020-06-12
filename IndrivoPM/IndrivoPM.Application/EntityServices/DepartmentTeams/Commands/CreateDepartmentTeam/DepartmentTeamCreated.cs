using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.CreateDepartmentTeam
{
    public class DepartmentTeamCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any department team is created.";

        public static string GroupName { get; } = "Administration_Team";

        public static string PlatformName { get; } = "ADM";

        public class DepartmentTeamCreatedHandler : INotificationHandler<DepartmentTeamCreated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public DepartmentTeamCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(DepartmentTeamCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"New team {notification.PrimaryEntityName} added.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.GroupEntityId,
                        Action = "Details",
                        Controller = "Department"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}

