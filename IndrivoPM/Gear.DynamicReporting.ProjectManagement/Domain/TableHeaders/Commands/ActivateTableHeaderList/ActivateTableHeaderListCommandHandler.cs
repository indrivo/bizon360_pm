using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.ReportEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeaderList
{
    public class ActivateTableHeaderListCommandHandler : IRequestHandler<ActivateTableHeaderListCommand>
    {
        private readonly IGearContext _context;

        public ActivateTableHeaderListCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ActivateTableHeaderListCommand request, CancellationToken cancellationToken)
        {
            var tableHeaderNames = await _context.ReportTableHeaders
                .Include(x => x.TableHeader)
                .Where(x => request.TableHeaderList.Contains(x.TableHeaderId))
                .Select(x => x.TableHeader.Name).ToListAsync(cancellationToken);

            var tableHeaders = await _context.ReportTableHeaders
                .Include(x => x.TableHeader)
                .Where(x => x.ReportId == request.ReportId
                            && x.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            foreach (var item in tableHeaders)
            {
                item.Active = tableHeaderNames.Contains(item.TableHeader.Name);
            }

            _context.ReportTableHeaders.UpdateRange(tableHeaders);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
