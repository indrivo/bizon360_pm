using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails
{
    public class GetUserReportDetailsQueryHandler : IRequestHandler<GetUserReportDetailsQuery, UserReportDetailModel>
    {
        private readonly IGearContext _context;

        public GetUserReportDetailsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportDetailModel> Handle(GetUserReportDetailsQuery request, CancellationToken cancellationToken)
        {
            var userReport = await _context.UserReports
                .Include(x => x.ApplicationUser)
                .Include(x => x.Report).ThenInclude(x => x.ActivityStatusFilters)
                .Include(x => x.Report).ThenInclude(x => x.AllowedFiltersByUser)
                .Include(x => x.Report).ThenInclude(x => x.UserDateFilters)
                .Include(x => x.Report).ThenInclude(x => x.UserGuidFilters)
                .Include(x => x.Report).ThenInclude(x => x.UserProjectStatusFilters)
                .Include(x => x.Report).ThenInclude(x => x.ReportTableHeaders)
                .FirstOrDefaultAsync(x => x.ReportId == request.ReportId && x.UserId == request.UserId, cancellationToken);
            if (userReport == null)
                throw new NotFoundException(nameof(UserReport<Guid>), new {request.ReportId, request.UserId });

            return UserReportDetailModel.Create(userReport);
        }
    }
}
