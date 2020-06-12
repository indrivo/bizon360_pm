using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.UpdateTableHeader
{
    public class UpdateTableHeaderCommandHandler : IRequestHandler<UpdateTableHeaderCommand>
    {
        private readonly IGearContext _context;

        public UpdateTableHeaderCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTableHeaderCommand request, CancellationToken cancellationToken)
        {
            var tableHeader = await _context.TableHeaders.FindAsync(request.Id);
            if (tableHeader == null)
                throw new NotFoundException(nameof(TableHeader<Guid>), request.Id);

            tableHeader.Name = request.Name;
            tableHeader.Active = request.Active;
            tableHeader.Deletable = request.Deletable;
            tableHeader.Order = request.Order;
            tableHeader.Width = request.Width;

            await Task.Run(() =>
            {
                _context.TableHeaders.Update(tableHeader);
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
