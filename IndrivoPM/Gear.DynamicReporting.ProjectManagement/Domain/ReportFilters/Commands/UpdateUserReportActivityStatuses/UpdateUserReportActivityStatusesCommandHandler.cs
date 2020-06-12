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

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportActivityStatuses
{
    public class UpdateUserReportActivityStatusesCommandHandler : IRequestHandler<UpdateUserReportActivityStatusesCommand<Guid>>
    {
        private readonly IGearContext _context;

        public UpdateUserReportActivityStatusesCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserReportActivityStatusesCommand<Guid> request, CancellationToken cancellationToken)
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

            await Task.Run(() =>
            {
                var activityStatuses =
                    _context.UserActivityStatusFilters.Where(x =>
                        x.ReportId == request.ReportId && x.UserId == request.UserId);

                _context.UserActivityStatusFilters.RemoveRange(activityStatuses);
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var userReportActivityStatuses = request.ActivityStatuses.Distinct()
                .Select(activityStatus => new UserActivityStatusFilter<Guid>
                {
                    Id = Guid.NewGuid(),
                    ActivityStatus = activityStatus,
                    ReportId = request.ReportId,
                    UserId = request.UserId
                }).ToList();

            await _context.UserActivityStatusFilters.AddRangeAsync(userReportActivityStatuses, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
