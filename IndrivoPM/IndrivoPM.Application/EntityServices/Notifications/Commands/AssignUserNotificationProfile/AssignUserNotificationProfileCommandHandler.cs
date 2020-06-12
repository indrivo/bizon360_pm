using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.AssignUserNotificationProfile
{
    internal class AssignUserNotificationProfileCommandHandler : IRequestHandler<AssignUserNotificationProfileCommand>
    {
        private readonly INotificationProfileService _service;

        public AssignUserNotificationProfileCommandHandler(INotificationProfileService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(AssignUserNotificationProfileCommand request, CancellationToken cancellationToken)
        {
            await _service.AssignUserNotificationProfile(new NotificationProfileModelDto()
            {
                Id = request.Id,
                Users = request.UserListIds,
            });

            return Unit.Value;
        }
    }
}
