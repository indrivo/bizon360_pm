using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Extensions.Notifications;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.AddRecruitmentStage
{
    public class RecruitmentStageCreated : MediatorNotification
    {
        public class RecruitmentStageCreatedHandler : INotificationHandler<RecruitmentStageCreated>
        {
            private readonly INotificationService _notification;
            private readonly IGearContext _context;

            public RecruitmentStageCreatedHandler(INotificationService notification, IGearContext context)
            {
                _notification = notification;
                _context = context;
            }

            /// <summary>
            /// Send notification on recruitment stage create
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(RecruitmentStageCreated notification, CancellationToken cancellationToken)
            {
                await notification.GetUsersFromProfilesAsync(_context, _notification);
                var message = notification.CreateNotificationMessage();

                if (message.To != "")
                {
                    message.Body =
                        $"A new stage was added to pipeline {notification.PrimaryEntityName}.";
                    message.MessageRedirectAction = new MessageRedirectAction()
                    {
                        Id = notification.PrimaryEntityId,
                        Action = "Index",
                        Controller = "RecruitmentStage"
                    };
                    await _notification.SendAsync(message);
                }
            }
        }
    }
}
