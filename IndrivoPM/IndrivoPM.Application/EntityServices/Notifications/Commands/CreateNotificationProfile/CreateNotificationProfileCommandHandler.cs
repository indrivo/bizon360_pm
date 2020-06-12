using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.CreateNotificationProfile
{
    public class CreateNotificationProfileCommandHandler : IRequestHandler<CreateNotificationProfileCommand>
    {
        private readonly INotificationProfileService _service;

        public CreateNotificationProfileCommandHandler(INotificationProfileService service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(CreateNotificationProfileCommand request, CancellationToken cancellationToken)
        {

            await _service.CreateNotificationProfile( new NotificationProfileModelDto()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Events = request.EventList,
                Users = request.UserList
            });
            return Unit.Value;
        }
    }
}
