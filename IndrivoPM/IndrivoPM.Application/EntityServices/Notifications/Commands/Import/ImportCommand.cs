using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.Import
{
    public class ImportCommand : IRequest
    {
        public Assembly Assembly { get; set; }

        public class ImportCommandHandler :  IRequestHandler<ImportCommand>
        {
            private readonly IEventService _eventService;

            public ImportCommandHandler(IEventService eventService)
            {
                _eventService = eventService;
            }

            public async Task<Unit> Handle(ImportCommand request, CancellationToken cancellationToken)
            {
                await _eventService.ImportAndCreateEvents(request.Assembly);

                return Unit.Value;
            }
        }
    }
}
