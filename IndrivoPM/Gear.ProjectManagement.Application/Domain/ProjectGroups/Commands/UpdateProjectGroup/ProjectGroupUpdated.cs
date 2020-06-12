using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.UpdateProjectGroup
{
    public class ProjectGroupUpdated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any project group is updated.";

        public static string GroupName { get; } = "Project_ProjectGroup";

        public static string PlatformName { get; } = "PM";

        public string Changes { get; set; }
        public class ProjectGroupUpdatedHandler : INotificationHandler<ProjectGroupUpdated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;

            public ProjectGroupUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on project group update
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(ProjectGroupUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body = $"Project group \"{notification.PrimaryEntityName}\" updated with changes: " + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Index",
                        Controller = "Projects"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
