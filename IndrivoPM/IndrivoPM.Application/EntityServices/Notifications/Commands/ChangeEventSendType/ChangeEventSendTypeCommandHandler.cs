using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.ChangeEventSendType
{
    internal class ChangeEventSendTypeCommandHandler : IRequestHandler<ChangeEventSendTypeCommand>
    {
        private readonly IEventService _eventService;

        public ChangeEventSendTypeCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Unit> Handle(ChangeEventSendTypeCommand request, CancellationToken cancellationToken)
        {
            var ev = await _eventService.GetEventModel(request.Id);

            if (ev.PropagationTypes.Contains(request.Type))
            {
                ev.PropagationTypes.Remove(request.Type);
            }
            else
            {
                ev.PropagationTypes.Add(request.Type);
            }

            await _eventService.UpdateEvent(ev);

            return Unit.Value;
        }
    }
}
