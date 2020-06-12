using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers
{
    public class GetProjectActivityTypeTrackersQueryHandler : IRequestHandler<GetProjectActivityTypeTrackersQuery, ProjectActivityTrackersListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectActivityTypeTrackersQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectActivityTrackersListViewModel> Handle(GetProjectActivityTypeTrackersQuery request, CancellationToken cancellationToken)
        {
            var businessCaseException = new List<string>();

            var project = await _context.Projects.FindAsync(request.ProjectId);
            if (project == null)
                businessCaseException.Add($"{nameof(Project)} entity with key {request.ProjectId} was not found.");

            var activityType = await _context.ActivityTypes.FindAsync(request.ActivityTypeId);
            if (activityType == null)
                businessCaseException.Add($"{nameof(ActivityType)} entity with key {request.ActivityTypeId} was not found.");

            if (businessCaseException.Any() || activityType == null)
                throw new BusinessUseCaseException(businessCaseException);

            var filteredActivities = _context.Activities
                .Where(x => x.ProjectId == request.ProjectId && x.ActivityTypeId == request.ActivityTypeId);

            var loggedTimes = _context.LoggedTimes
                .Include(x => x.Tracker)
                .Include(x => x.Activity)
                .Where(x => x.Activity.ActivityTypeId == request.ActivityTypeId &&
                            filteredActivities.Select(pk => pk.Id).Contains(x.ActivityId));

            var result = await loggedTimes.Select(x => new ActivityTrackerDto
            {
                TrackerTypeId = x.TrackerId,
                TrackerName = x.Tracker.Name,
                LoggedTime = x.Time
            }).ToListAsync(cancellationToken);

            return new ProjectActivityTrackersListViewModel
            {
                ActivityTypeName = activityType.Name,
                ActivityTrackers = result.GroupBy(x => x.TrackerTypeId)
                    .ToDictionary(x => x.Key, x => x.ToList())
                    .Select(ActivityTrackerModel.Create).ToList()
            };
        }
    }
}
