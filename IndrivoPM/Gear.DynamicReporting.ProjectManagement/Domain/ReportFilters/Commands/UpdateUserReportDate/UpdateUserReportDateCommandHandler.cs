using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities.FilterEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportDate
{
    public class UpdateUserReportDateCommandHandler : IRequestHandler<UpdateUserReportDateCommand<Guid>>
    {
        private readonly IGearContext _context;

        public UpdateUserReportDateCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserReportDateCommand<Guid> request, CancellationToken cancellationToken)
        {
            var userReportDateFilter = await _context.UserDateFilters
                .FirstOrDefaultAsync(x => x.ReportId == request.ReportId
                                          && x.UserId == request.UserId && x.FilterType == request.FilterType, cancellationToken);

            if (userReportDateFilter == null)
            {
                throw new NotFoundException(nameof(UserDateFilter<Guid>),
                    new {request.ReportId, request.UserId, request.FilterType});
            }

            await Task.Run(() =>
            {
                userReportDateFilter.Date = request.Date;
                _context.UserDateFilters.Update(userReportDateFilter);
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
