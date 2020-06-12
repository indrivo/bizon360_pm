using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetUserReportHeaders
{
    public class GetUserReportHeadersQueryHandler : IRequestHandler<GetUserReportHeadersQuery, UserReportHeaderListViewModels>
    {
        private readonly IGearContext _context;

        public GetUserReportHeadersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportHeaderListViewModels> Handle(GetUserReportHeadersQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.ApplicationUsers.FindAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), request.UserId);

            var report = await _context.Reports.FindAsync(request.ReportId);
            if (report == null)
                throw new NotFoundException(nameof(Report<Guid>), request.ReportId);

            var userReportHeaders = await _context.ReportTableHeaders
                .Include(x => x.ApplicationUser)
                .Include(x => x.Report)
                .Include(x => x.TableHeader)
                .Where(x => x.UserId == request.UserId && x.ReportId == request.ReportId)
                .ToListAsync(cancellationToken);
            
            if (userReportHeaders == null)
                throw new NotFoundException(nameof(UserReport<Guid>), new {request.UserId, request.ReportId});

            var result = new UserReportHeaderListViewModels
            {
                UserId = request.UserId,
                UserName = $"{user.FirstName} {user.LastName}",
                ReportId = request.ReportId,
                ReportName = report.Name,
                TableHeaders = userReportHeaders.Select(TableHeaderDetailModel.Create).ToList()
            };

            return result;
        }
    }
}
