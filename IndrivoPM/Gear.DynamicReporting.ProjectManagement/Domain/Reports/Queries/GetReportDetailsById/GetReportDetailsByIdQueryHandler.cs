using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsById
{
    public class GetReportDetailsByIdQueryHandler : IRequestHandler<GetReportDetailsByIdQuery, ReportDetailModel>
    {
        private readonly IGearContext _context;

        public GetReportDetailsByIdQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ReportDetailModel> Handle(GetReportDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(x => x.Id == request.ReportId, cancellationToken);
            if (report == null)
                throw new NotFoundException(nameof(Report<Guid>), request.ReportId);

            return ReportDetailModel.Create(report);
        }
    }
}
