using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.DeleteNotificationProfile
{
    public class DeleteNotificationProfileCommandHandler : IRequestHandler<DeleteNotificationProfileCommand>
    {
        private readonly INotificationProfileService _service;

        public DeleteNotificationProfileCommandHandler(INotificationProfileService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(DeleteNotificationProfileCommand request, CancellationToken cancellationToken)
        {
            foreach (var profileId in request.Ids)
            {
                await _service.DeleteNotificationProfile(profileId);
            }

            return Unit.Value;
        }
    }
}
