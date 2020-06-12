using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.RemoveReportOfUser
{
    public class RemoveReportOfUserCommandHandler : IRequestHandler<RemoveReportOfUserCommand<Guid>>
    {
        private readonly IGearContext _context;

        public RemoveReportOfUserCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveReportOfUserCommand<Guid> request, CancellationToken cancellationToken)
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

            var userReport = await _context.UserReports.FindAsync(new { request.UserId, request.ReportId });
            if (userReport == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(UserReport<Guid>)} does not contain the {request.ReportId}, {request.UserId} key combination!");
            }

            if (businessUseCaseExceptions.Any())
            {
                throw new BusinessUseCaseException(businessUseCaseExceptions);
            }

            if (userReport != null) 
                _context.UserReports.Remove(userReport);

            return Unit.Value;
        }
    }
}
