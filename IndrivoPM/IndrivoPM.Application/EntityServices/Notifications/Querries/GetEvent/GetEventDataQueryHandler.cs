using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetEvent
{
    public class GetEventDataQueryHandler : IRequestHandler<GetEventDataQuery, EventDto>
    {

        private readonly IEventService _eventService;

        public GetEventDataQueryHandler(IEventService eventService)
        {
            _eventService = eventService;
        }


        public async Task<EventDto> Handle(GetEventDataQuery request, CancellationToken cancellationToken)
        {
            return await _eventService.GetEventModel(request.Id);
        }
    }
}
