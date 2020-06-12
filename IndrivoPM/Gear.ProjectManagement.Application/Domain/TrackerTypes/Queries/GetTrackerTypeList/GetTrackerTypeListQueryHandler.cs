using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList
{
    public class GetTrackerTypeListQueryHandler : IRequestHandler<GetTrackerTypeListQuery, TrackerTypeListViewModel>
    {
        private readonly IGearContext _context;

        public GetTrackerTypeListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TrackerTypeListViewModel> Handle(GetTrackerTypeListQuery request, CancellationToken cancellationToken)
        {
            if (request.ActivityTypeId != Guid.Empty && request.ActivityTypeId != null)
            {
                return new TrackerTypeListViewModel
                {
                    Trackers = await _context.TrackerTypes.OrderBy(x => x.RowOrder)
                    .Include(x => x.ActivityType).Where(x => x.ActivityTypeId == request.ActivityTypeId)
                    .Select(x => TrackerTypeLookupModel.Create(x))
                    .ToListAsync(cancellationToken)
                };
            }

            return new TrackerTypeListViewModel
            {
                Trackers = await _context.TrackerTypes.OrderBy(x => x.RowOrder)
                    .Select(x => TrackerTypeLookupModel.Create(x))
                    .ToListAsync(cancellationToken)
            };
        }
    }
}
