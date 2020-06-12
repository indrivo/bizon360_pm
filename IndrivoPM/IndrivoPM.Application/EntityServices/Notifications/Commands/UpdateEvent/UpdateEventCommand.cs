using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateEvent
{
    public class UpdateEventCommand : IRequest
    {
        public EventDto Event { get; set; }
    }
}
