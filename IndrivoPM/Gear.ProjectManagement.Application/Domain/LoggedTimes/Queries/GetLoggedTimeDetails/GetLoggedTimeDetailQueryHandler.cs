using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeDetails
{
    public class GetLoggedTimeDetailQueryHandler : IRequestHandler<GetLoggedTimeDetailQuery, LoggedTimeDetailModel>
    {
        private readonly IGearContext _context;

        public GetLoggedTimeDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<LoggedTimeDetailModel> Handle(GetLoggedTimeDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = (await _context.LoggedTimes
                .Include(x => x.Activity)
                .Include(x => x.User)
                .Include(x => x.Tracker)
                .FirstAsync(x => x.Id == request.Id, cancellationToken));

            if (entity == null)
            {
                throw new NotFoundException(nameof(LoggedTime), request.Id);
            }

            return LoggedTimeDetailModel.Create(entity);
        }
    }
}
