using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IEventService _eventService;

        public UpdateEventCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            await _eventService.UpdateEvent(request.Event);

            return Unit.Value;
        }
    }
}
