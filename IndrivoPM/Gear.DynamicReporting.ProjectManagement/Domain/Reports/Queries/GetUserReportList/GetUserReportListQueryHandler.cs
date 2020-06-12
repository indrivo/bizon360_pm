using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportList
{
    public class GetUserReportListQueryHandler : IRequestHandler<GetUserReportListQuery, UserReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetUserReportListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportListViewModel> Handle(GetUserReportListQuery request, CancellationToken cancellationToken)
        {
            var userReports = await _context.UserReports
                .Include(x => x.ApplicationUser)
                .Include(x => x.Report).ThenInclude(x => x.ActivityStatusFilters)
                .Include(x => x.Report).ThenInclude(x => x.AllowedFiltersByUser)
                .Include(x => x.Report).ThenInclude(x => x.UserDateFilters)
                .Include(x => x.Report).ThenInclude(x => x.UserGuidFilters)
                .Include(x => x.Report).ThenInclude(x => x.UserProjectStatusFilters)
                .Include(x => x.Report).ThenInclude(x => x.ReportTableHeaders)
                .Where(x => x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            if (userReports == null)
                throw new NotFoundException(nameof(UserReport<Guid>), new { request.UserId });

            var user = await _context.ApplicationUsers.FindAsync(request.UserId);

            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), new { request.UserId });

            var result = new UserReportListViewModel
            {
                UserId = request.UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                UserReports = userReports.Select(UserReportModel.Create).ToList()
            };

            return result;
        }
    }
}
