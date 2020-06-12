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

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportGuids
{
    public class UpdateUserReportGuidsCommandHandler : IRequestHandler<UpdateUserReportGuidsCommand<Guid>>
    {
        private readonly IGearContext _context;

        public UpdateUserReportGuidsCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserReportGuidsCommand<Guid> request, CancellationToken cancellationToken)
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

            var guids =
                await _context.UserGuidFilters.Where(x =>
                        x.ReportId == request.ReportId && x.UserId == request.UserId && x.FilterType == request.FilterType)
                    .Select(x => x.EntityId)
                    .ToListAsync(cancellationToken);

            var exceptGuids = guids.Except(request.GuidList).ToList();
            var filtersToEliminate = _context.UserGuidFilters
                .Where(x => exceptGuids.Contains(x.EntityId)
                            && x.ReportId == request.ReportId && x.UserId == request.UserId
                            && x.FilterType == request.FilterType).ToList();

            _context.UserGuidFilters.RemoveRange(filtersToEliminate);

            await _context.SaveChangesAsync(cancellationToken);

            var userReportGuids = request.GuidList.Distinct()
                .Where(x => !exceptGuids.Contains(x))
                .Select(guid => new UserGuidFilter<Guid>
                {
                    Id = Guid.NewGuid(),
                    EntityId = guid,
                    ReportId = request.ReportId,
                    UserId = request.UserId,
                    FilterType = request.FilterType
                }).ToList();

            if (_context.UserGuidFilters != null)
                await _context.UserGuidFilters.AddRangeAsync(userReportGuids, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
