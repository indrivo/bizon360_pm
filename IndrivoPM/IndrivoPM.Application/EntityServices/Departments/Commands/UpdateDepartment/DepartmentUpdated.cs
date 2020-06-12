using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateDepartment
{
    public class DepartmentUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any department is updated.";

        public static string GroupName { get; } = "Administration_Department";

        public static string PlatformName { get; } = "ADM";

        public class DepartmentUpdatedHandler : INotificationHandler<DepartmentUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public DepartmentUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(DepartmentUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.Body = $"Department  \"{notification.PrimaryEntityName}\" was updated.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Controller = "Departments",
                        Action = "Details"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
