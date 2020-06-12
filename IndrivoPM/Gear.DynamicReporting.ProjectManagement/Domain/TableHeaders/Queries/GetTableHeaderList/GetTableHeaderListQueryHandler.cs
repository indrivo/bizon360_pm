using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderList
{
    public class GetTableHeaderListQueryHandler : IRequestHandler<GetTableHeaderListQuery, TableHeaderListViewModel>
    {
        private readonly IGearContext _context;

        public GetTableHeaderListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TableHeaderListViewModel> Handle(GetTableHeaderListQuery request, CancellationToken cancellationToken)
        {
            var tableHeaders = await _context.TableHeaders.ToListAsync(cancellationToken);

            return new TableHeaderListViewModel
            {
                TableHeaderList = tableHeaders.Select(TableHeaderDetailModel.Create).ToList()
            };
        }
    }
}
