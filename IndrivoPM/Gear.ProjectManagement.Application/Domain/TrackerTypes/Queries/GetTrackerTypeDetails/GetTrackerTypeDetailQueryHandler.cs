using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeDetails
{
    public class GetTrackerTypeDetailQueryHandler : IRequestHandler<GetTrackerTypeDetailQuery, TrackerTypeDetailModel>
    {
        private readonly IGearContext _context;

        public GetTrackerTypeDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TrackerTypeDetailModel> Handle(GetTrackerTypeDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.TrackerTypes
                .Include(x => x.ActivityType)
                .Include(x => x.LoggedTimes)
                .FirstOrDefaultAsync(x =>x.Id == request.Id, cancellationToken: cancellationToken);

            if (entity == null) throw new NotFoundException(nameof(TrackerTypes), request.Id);

            return TrackerTypeDetailModel.Create(entity);
        }
    }
}
