using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportDate
{
    public class GetUserReportDateQueryHandler : IRequestHandler<GetUserReportDateQuery, UserReportDate>
    {
        private readonly IGearContext _context;

        public GetUserReportDateQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportDate> Handle(GetUserReportDateQuery request, CancellationToken cancellationToken)
        {
            var userDateFilter = await _context.UserDateFilters
                .FirstOrDefaultAsync(x => x.UserId == request.UserId
            && x.ReportId == request.ReportId && x.FilterType == request.FilterType, cancellationToken);

            if (userDateFilter == null)
                return null;

            return new UserReportDate
            {
                Date = userDateFilter.Date
            };
        }
    }
}
