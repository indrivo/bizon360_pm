using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.UpdateReport
{
    public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand>
    {
        private readonly IGearContext _context;

        public UpdateReportCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            var doesReportWithSameNameExist = await _context.Reports.AnyAsync(x => x.Name == request.Name, cancellationToken);
            if (doesReportWithSameNameExist)
                throw new BusinessUseCaseException(new List<string> {$"The report with same name ({request.Name}) already exists!"});
            
            var report = await _context.Reports.FindAsync(request.Id, cancellationToken);
            if (report == null)
                throw new NotFoundException(nameof(Report<Guid>), request.Id);

            await Task.Run(() =>
            {
                report.Name = request.Name;
                _context.Reports.Update(report);
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
