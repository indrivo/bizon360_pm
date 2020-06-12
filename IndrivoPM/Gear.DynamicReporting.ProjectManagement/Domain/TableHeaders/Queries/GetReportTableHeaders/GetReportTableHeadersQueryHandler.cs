using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetReportTableHeaders
{
    public class GetReportTableHeadersQueryHandler : IRequestHandler<GetReportTableHeadersQuery, ReportTableHeaderListViewModel>
    {
        private readonly IGearContext _context;

        public GetReportTableHeadersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ReportTableHeaderListViewModel> Handle(GetReportTableHeadersQuery request, CancellationToken cancellationToken)
        {
            var tableHeaders = await _context.ReportTableHeaders
                .Include(x => x.TableHeader)
                .Where(x => x.ReportId == request.ReportId)
                .ToListAsync(cancellationToken);

            return new ReportTableHeaderListViewModel
            {
                TableHeaders = tableHeaders.Select(TableHeaderDetailModel.Create).ToList()
            };
        }
    }
}
