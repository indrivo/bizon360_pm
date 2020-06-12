using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.AddUserToReport
{
    public class AddUserToReportCommandHandler : IRequestHandler<AddUserToReportCommand<Guid>>
    {
        private readonly IGearContext _context;

        public AddUserToReportCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserToReportCommand<Guid> request, CancellationToken cancellationToken)
        {
            var businessUseCaseExceptions = new List<string>();

            var user = await _context.ApplicationUsers.FindAsync(request.UserId);
            if (user == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(ApplicationUser)} does not contain the {request.UserId} key!");
            }

            var report = await _context.Reports.FindAsync(request.ReportId);
            if (report == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(Report<Guid>)} does not contain the {request.ReportId} key!");
            }

            if (businessUseCaseExceptions.Any())
            {
                throw new BusinessUseCaseException(businessUseCaseExceptions);
            }

            var userReport = new UserReport<Guid>
            {
                UserId = request.UserId,
                ReportId = request.ReportId
            };

            await _context.UserReports.AddAsync(userReport, cancellationToken);

            return Unit.Value;
        }
    }
}
