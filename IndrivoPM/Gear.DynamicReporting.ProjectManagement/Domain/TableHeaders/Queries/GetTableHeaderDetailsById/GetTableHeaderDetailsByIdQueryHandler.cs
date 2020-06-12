using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById
{
    public class GetTableHeaderDetailsByIdQueryHandler : IRequestHandler<GetTableHeaderDetailsByIdQuery, TableHeaderDetailModel>
    {
        private readonly IGearContext _context;

        public GetTableHeaderDetailsByIdQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TableHeaderDetailModel> Handle(GetTableHeaderDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var tableHeader = await _context.TableHeaders.FindAsync(request.TableHeaderId);
            if (tableHeader == null)
                throw new NotFoundException(nameof(TableHeader<Guid>), request.TableHeaderId);

            return TableHeaderDetailModel.Create(tableHeader);
        }
    }
}
