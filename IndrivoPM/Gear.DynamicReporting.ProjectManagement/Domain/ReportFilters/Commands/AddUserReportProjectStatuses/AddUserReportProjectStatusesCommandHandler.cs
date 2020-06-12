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

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportProjectStatuses
{
    public class AddUserReportProjectStatusesCommandHandler : IRequestHandler<AddUserReportProjectStatusesCommand<Guid>>
    {
        private readonly IGearContext _context;

        public AddUserReportProjectStatusesCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserReportProjectStatusesCommand<Guid> request, CancellationToken cancellationToken)
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

            var projectStatuses =
                await _context.UserProjectStatusFilters.Where(x =>
                        x.ReportId == request.ReportId && x.UserId == request.UserId)
                    .Select(x => x.ProjectStatus)
                    .ToListAsync(cancellationToken);

            var userReportProjectStatuses = request.ProjectStatuses.Distinct()
                .Where(x => !projectStatuses.Contains(x))
                .Select(projectStatus => new UserProjectStatusFilter<Guid>
                {
                    Id = Guid.NewGuid(),
                    ProjectStatus = projectStatus,
                    ReportId = request.ReportId,
                    UserId = request.UserId
                }).ToList();

            if (_context.UserProjectStatusFilters != null)
                await _context.UserProjectStatusFilters.AddRangeAsync(userReportProjectStatuses, cancellationToken);

            return Unit.Value;
        }
    }
}
