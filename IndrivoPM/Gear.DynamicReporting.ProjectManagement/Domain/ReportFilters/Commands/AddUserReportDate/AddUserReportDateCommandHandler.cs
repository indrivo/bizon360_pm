using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities.FilterEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportDate
{
    public class AddUserReportDateCommandHandler : IRequestHandler<AddUserReportDateCommand<Guid>>
    {
        private readonly IGearContext _context;

        public AddUserReportDateCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserReportDateCommand<Guid> request, CancellationToken cancellationToken)
        {
            var userReportDateFilter = await _context.UserDateFilters
                .FirstOrDefaultAsync(x => x.ReportId == request.ReportId 
                    && x.UserId == request.UserId && x.FilterType == request.FilterType, cancellationToken);

            if (userReportDateFilter != null)
            {
                throw new BusinessUseCaseException(new List<string> {$"The object {nameof(UserDateFilter<Guid>)} already has value. Cannot add another one!"});
            }

            var result = new UserDateFilter<Guid>
            {
                ReportId = request.ReportId,
                UserId = request.UserId,
                FilterType = request.FilterType,
                Date = request.Date
            };
            
            await _context.UserDateFilters.AddAsync(result, cancellationToken);

            return Unit.Value;
        }
    }
}
