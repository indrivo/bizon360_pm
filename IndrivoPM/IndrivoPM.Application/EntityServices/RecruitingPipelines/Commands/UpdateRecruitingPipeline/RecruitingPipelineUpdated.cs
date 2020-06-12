using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.UpdateRecruitingPipeline
{
    public class RecruitingPipelineUpdated : MediatorNotification
    {
        public string Changes { get; set; }

        public class RecruitingPipelineUpdatedHandler : INotificationHandler<RecruitingPipelineUpdated>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public RecruitingPipelineUpdatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// end notification on recruiting pipeline update
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(RecruitingPipelineUpdated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body = $"Recruiting pipeline \"{notification.PrimaryEntityName}\" updated by {notification.UserName} with changes: " + notification.Changes;
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Index",
                        Controller = "RecruitingPipeline"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
