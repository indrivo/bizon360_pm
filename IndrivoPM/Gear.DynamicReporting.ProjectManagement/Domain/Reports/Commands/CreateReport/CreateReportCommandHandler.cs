using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.CreateReport
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand>
    {
        private readonly IGearContext _context;

        public CreateReportCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var doesSameReportExist = await _context.Reports.AnyAsync(x => x.Name == request.Name, cancellationToken);
            if (doesSameReportExist)
                throw new BusinessUseCaseException(new List<string> {$"The report with {request.Name} already exists!"});

            var report = new Report<Guid>
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _context.Reports.AddAsync(report, cancellationToken);

            return Unit.Value;
        }
    }
}
