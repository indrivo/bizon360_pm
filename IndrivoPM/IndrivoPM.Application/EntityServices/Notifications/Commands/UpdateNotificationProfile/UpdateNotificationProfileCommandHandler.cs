using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateNotificationProfile
{
    public class UpdateNotificationProfileCommandHandler : IRequestHandler<UpdateNotificationProfileCommand>
    {
        private readonly INotificationProfileService _service;

        public UpdateNotificationProfileCommandHandler(INotificationProfileService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(UpdateNotificationProfileCommand request, CancellationToken cancellationToken)
        {
            await _service.UpdateNotificationProfile( new NotificationProfileModelDto()
            {
                Id = request.Id,
                Name = request.Name,
                Users = request.UserList,
                Events = request.EventList
            });
            return Unit.Value;
        }
    }
}
