using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using Gear.Domain.ReportEntities.FilterEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportActivityStatuses
{
    public class AddUserReportActivityStatusesCommandHandler : IRequestHandler<AddUserReportActivityStatusesCommand<Guid>>
    {
        private readonly IGearContext _context;

        public AddUserReportActivityStatusesCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserReportActivityStatusesCommand<Guid> request, CancellationToken cancellationToken)
        {
            var businessUseCaseExceptions = new List<string>();
            
            var report = await _context.Reports.FindAsync(request.ReportId);
            if (report == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(Report<Guid>)} has no {request.ReportId} key!");
            }

            var user = await _context.ApplicationUsers.FindAsync(request.UserId);
            if (user == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(ApplicationUser)} has no {request.UserId} key!");
            }

            if (businessUseCaseExceptions.Any())
                throw new BusinessUseCaseException(businessUseCaseExceptions);

            var activityStatuses =
                await _context.UserActivityStatusFilters.Where(x =>
                        x.ReportId == request.ReportId && x.UserId == request.UserId)
                    .Select(x => x.ActivityStatus)
                    .ToListAsync(cancellationToken);

            var userReportActivityStatuses = request.ActivityStatuses.Distinct()
                .Where(x => !activityStatuses.Contains(x))
                .Select(activityStatus => new UserActivityStatusFilter<Guid>
                {
                    Id = Guid.NewGuid(),
                    ActivityStatus = activityStatus,
                    ReportId = request.ReportId,
                    UserId = request.UserId
                }).ToList();

            if (_context.UserActivityStatusFilters != null)
                await _context.UserActivityStatusFilters.AddRangeAsync(userReportActivityStatuses, cancellationToken);

            return Unit.Value;
        }
    }
}
