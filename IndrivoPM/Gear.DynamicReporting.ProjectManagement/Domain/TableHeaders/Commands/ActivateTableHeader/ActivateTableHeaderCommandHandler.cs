using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using Gear.Notifications.Abstractions.Common.Exceptions;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeader
{
    class ActivateTableHeaderCommandHandler : IRequestHandler<ActivateTableHeaderCommand>
    {
        private readonly IGearContext _context;

        public ActivateTableHeaderCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ActivateTableHeaderCommand request, CancellationToken cancellationToken)
        {
            var tableHeader = await _context.TableHeaders.FindAsync(request.Id);
            if (tableHeader == null)
                throw new NotFoundException(nameof(TableHeader<Guid>), request.Id);

            await Task.Run(() =>
            {
                tableHeader.Active = true;
                _context.TableHeaders.Update(tableHeader);
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
