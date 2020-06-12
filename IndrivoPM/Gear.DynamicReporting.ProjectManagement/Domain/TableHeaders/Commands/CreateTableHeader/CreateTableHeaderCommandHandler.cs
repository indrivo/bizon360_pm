using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.CreateTableHeader
{
    public class CreateTableHeaderCommandHandler : IRequestHandler<CreateTableHeaderCommand>
    {
        private readonly IGearContext _context;

        public CreateTableHeaderCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateTableHeaderCommand request, CancellationToken cancellationToken)
        {
            var tableHeader = new TableHeader<Guid>
            {
                Id = Guid.NewGuid(),
                Active = request.Active,
                Deletable = request.Deletable,
                Name = request.Name,
                Order = request.Order,
                Width = request.Width
            };

            await _context.TableHeaders.AddAsync(tableHeader, cancellationToken);

            return Unit.Value;
        }
    }
}
