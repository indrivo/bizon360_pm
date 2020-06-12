using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityTrackers
{
    public class GetActivityTrackersQueryHandler : IRequestHandler<GetActivityTrackersQuery, TrackerTypeListViewModel>
    {
        private readonly IGearContext _context;

        public GetActivityTrackersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TrackerTypeListViewModel> Handle(GetActivityTrackersQuery request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);

            if (activity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            return new TrackerTypeListViewModel
            {
                Trackers = await _context.TrackerTypes
                    .Include(t => t.ActivityType)
                    .Where(t => t.ActivityTypeId == activity.ActivityTypeId)
                    .Select(t => TrackerTypeLookupModel.Create(t))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
