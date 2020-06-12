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

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.AddTableHeaderToUserReport
{
    public class AddTableHeaderToUserReportCommandHandler : IRequestHandler<AddTableHeaderToUserReportCommand<Guid>>
    {
        private readonly IGearContext _context;

        public AddTableHeaderToUserReportCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddTableHeaderToUserReportCommand<Guid> request, CancellationToken cancellationToken)
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

            var tableHeader = await _context.TableHeaders.FindAsync(request.TableHeaderId);
            if (tableHeader == null)
            {
                businessUseCaseExceptions.Add($"The object {nameof(TableHeader<Guid>)} with key {request.TableHeaderId} has not been found!");
            }

            if (businessUseCaseExceptions.Any())
                throw new BusinessUseCaseException(businessUseCaseExceptions);

            if (tableHeader != null && !tableHeader.Active)
            {
                businessUseCaseExceptions.Add($"This object {nameof(TableHeader<Guid>)} was deactivated and operations can't be performed on it!");
            }

            var userTableHeader = await _context.ReportTableHeaders
                .FindAsync(new {request.ReportId, request.TableHeaderId, request.UserId});
            if (userTableHeader != null)
            {
                businessUseCaseExceptions.Add($"The user's report already has this table header!");
            }

            if (businessUseCaseExceptions.Any())
                throw new BusinessUseCaseException(businessUseCaseExceptions);

            var userReportTableHeader = new ReportTableHeader<Guid>
            {
                ReportId = request.ReportId,
                UserId = request.UserId,
                TableHeaderId = request.TableHeaderId,
                Active = true
            };
            await _context.ReportTableHeaders.AddAsync(userReportTableHeader, cancellationToken);

            return Unit.Value;
        }
    }
}
