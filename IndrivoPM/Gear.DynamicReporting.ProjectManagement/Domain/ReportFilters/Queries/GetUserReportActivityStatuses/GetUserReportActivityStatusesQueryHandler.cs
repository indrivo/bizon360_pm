using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportActivityStatuses
{
    public class GetUserReportActivityStatusesQueryHandler : IRequestHandler<GetUserReportActivityStatusesQuery, UserReportActivityStatusesModel>
    {
        private readonly IGearContext _context;

        public GetUserReportActivityStatusesQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportActivityStatusesModel> Handle(GetUserReportActivityStatusesQuery request, CancellationToken cancellationToken)
        {
            var userActivityStatusFilters = await _context.UserActivityStatusFilters
                .Where(x => x.ReportId == request.ReportId && x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (userActivityStatusFilters == null)
                return null;

            return new UserReportActivityStatusesModel
            {
                ActivityStatuses = userActivityStatusFilters.Select(x => x.ActivityStatus).ToList()
            };
        }
    }
}
