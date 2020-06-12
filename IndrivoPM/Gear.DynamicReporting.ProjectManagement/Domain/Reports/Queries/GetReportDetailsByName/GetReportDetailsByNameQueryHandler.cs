using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName
{
    public class GetReportDetailsByNameQueryHandler : IRequestHandler<GetReportDetailsByNameQuery, ReportDetailModel>
    {
        private readonly IGearContext _context;

        public GetReportDetailsByNameQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ReportDetailModel> Handle(GetReportDetailsByNameQuery request, CancellationToken cancellationToken)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (report == null)
                throw new NotFoundException(nameof(Report<Guid>), request.Name);

            return ReportDetailModel.Create(report);
        }
    }
}
