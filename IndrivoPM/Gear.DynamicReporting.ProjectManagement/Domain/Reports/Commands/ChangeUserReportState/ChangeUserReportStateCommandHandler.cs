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

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.ChangeUserreportState
{
    public class ChangeUserReportStateCommandHandler : IRequestHandler<ChangeUserReportStateCommand>
    {
        private readonly IGearContext _context;

        public ChangeUserReportStateCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ChangeUserReportStateCommand request, CancellationToken cancellationToken)
        {
            var businessUseCaseExceptions = new List<string>();

            var user = await _context.ApplicationUsers.FindAsync(request.UserId);
            if (user == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(ApplicationUser)} with key {request.UserId} has not been found!");
            }

            var report = await _context.Reports.FindAsync(request.ReportId);
            if (report == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(Report<Guid>)} with key {request.ReportId} has not been found!");
            }

            if (businessUseCaseExceptions.Any())
                throw new BusinessUseCaseException(businessUseCaseExceptions);

            var userReport = await _context.UserReports.FindAsync(request.UserId, request.ReportId);
            if (userReport == null)
                throw new NotFoundException(nameof(UserReport<Guid>), new { request.UserId, request.ReportId });

            userReport.Active = request.Active;

            _context.UserReports.Update(userReport);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
