using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateJobPosition
{
    public class JobPositionUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any job position is updated.";

        public static string GroupName { get; } = "Administration_JobPosition";

        public static string PlatformName { get; } = "ADM";

        public class JobPositionUpdatedHandler : INotificationHandler<JobPositionUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public JobPositionUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            public async Task Handle(JobPositionUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.Body = $"Job position {notification.PrimaryEntityName} was updated";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Action = "Index",
                        Controller = "JobPositions"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
