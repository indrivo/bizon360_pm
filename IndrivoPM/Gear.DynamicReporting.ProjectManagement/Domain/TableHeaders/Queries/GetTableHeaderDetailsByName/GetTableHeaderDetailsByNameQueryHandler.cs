using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsByName
{
    public class GetTableHeaderDetailsByNameQueryHandler : IRequestHandler<GetTableHeaderDetailsByNameQuery, TableHeaderDetailModel>
    {
        private readonly IGearContext _context;

        public GetTableHeaderDetailsByNameQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TableHeaderDetailModel> Handle(GetTableHeaderDetailsByNameQuery request, CancellationToken cancellationToken)
        {
            var tableHeader =
                await _context.TableHeaders.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (tableHeader == null)
                throw new NotFoundException(nameof(TableHeader<Guid>), request.Name);

            return TableHeaderDetailModel.Create(tableHeader);
        }
    }
}
