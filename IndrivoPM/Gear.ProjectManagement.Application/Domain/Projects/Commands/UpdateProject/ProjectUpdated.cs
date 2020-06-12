using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.UpdateProject
{
    public class ProjectUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any project is updated.";

        public static string GroupName { get; } = "Project_Project";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }

        public class ProjectUpdatedHandler : INotificationHandler<ProjectUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ProjectUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on project update
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(ProjectUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Project \"{notification.PrimaryEntityName}\" updated with changes: " + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Details",
                        Controller = "Projects"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}