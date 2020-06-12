using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using Gear.Notifications.Abstractions.Common.Exceptions;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.DeactivateTableHeader
{
    public class DeactivateTableHeaderCommandHandler : IRequestHandler<DeactivateTableHeaderCommand>
    {
        private readonly IGearContext _context;

        public DeactivateTableHeaderCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeactivateTableHeaderCommand request, CancellationToken cancellationToken)
        {
            var tableHeader = await _context.TableHeaders.FindAsync(request.Id);
            if (tableHeader == null)
                throw new NotFoundException(nameof(TableHeader<Guid>), request.Id);

            await Task.Run(() => {
                if (tableHeader.Deletable)
                    tableHeader.Active = false;
            }, cancellationToken);
            
            return Unit.Value;
        }
    }
}
