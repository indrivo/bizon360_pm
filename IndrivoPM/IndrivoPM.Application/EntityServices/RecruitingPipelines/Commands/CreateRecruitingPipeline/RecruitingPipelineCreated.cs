using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.CreateRecruitingPipeline
{
    public class RecruitingPipelineCreated : MediatorNotification
    {
        public class RecruitingPipelineCreatedHandler : INotificationHandler<RecruitingPipelineCreated>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public RecruitingPipelineCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on recruiting pipeline create
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(RecruitingPipelineCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body =
                        $"Recruiting pipeline \"{notification.PrimaryEntityName}\" was created by {notification.UserName}.";
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
