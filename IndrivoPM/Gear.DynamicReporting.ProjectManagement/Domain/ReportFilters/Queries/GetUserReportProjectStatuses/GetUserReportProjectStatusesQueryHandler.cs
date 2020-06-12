using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportProjectStatuses
{
    public class GetUserReportProjectStatusesQueryHandler : IRequestHandler<GetUserReportProjectStatusesQuery, UserReportProjectStatuses>
    {
        private readonly IGearContext _context;

        public GetUserReportProjectStatusesQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportProjectStatuses> Handle(GetUserReportProjectStatusesQuery request, CancellationToken cancellationToken)
        {
            var userProjectStatusFilters = await _context.UserProjectStatusFilters
                .Where(x => x.ReportId == request.ReportId && x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (userProjectStatusFilters == null || !userProjectStatusFilters.Any())
                return null;

            return new UserReportProjectStatuses
            {
                ProjectStatuses = userProjectStatusFilters.Select(x => x.ProjectStatus).ToList()
            };
        }
    }
}
