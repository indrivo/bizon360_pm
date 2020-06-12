using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateDepartmentTeam
{
    public class DepartmentTeamUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any department team is updated.";

        public static string GroupName { get; } = "Administration_Team";

        public static string PlatformName { get; } = "ADM";

        public class DepartmentTeamUpdatedHandler : INotificationHandler<DepartmentTeamUpdated>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;


            public DepartmentTeamUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(DepartmentTeamUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Department team \"{notification.PrimaryEntityName}\" was updated.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Details",
                        Controller = "Activity"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
