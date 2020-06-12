using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.CreateProjectGroup
{
    public class ProjectGroupCreated : MediatorNotification
    {
        public static string EventName { get; } = "Notify if any project group is created.";

        public static string GroupName { get; } = "Project_ProjectGroup";

        public static string PlatformName { get; } = "PM";

        public class ProjectGroupCreatedHandler : INotificationHandler<ProjectGroupCreated>
        {
            private readonly INotificationService _notification;

            private readonly IGearContext _context;


            public ProjectGroupCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on project group create
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(ProjectGroupCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.From = notification.UserName;
                    message.PrimaryEntityGroupName = notification.GroupEntityName;
                    message.Body =
                        $"Project group \"{notification.PrimaryEntityName}\" was created.";
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
