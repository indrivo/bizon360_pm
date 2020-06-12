using System;
using Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityTrackers
{
    public class GetActivityTrackersQuery : IRequest<TrackerTypeListViewModel>
    {
        public Guid Id { get; set; }
    }
}
