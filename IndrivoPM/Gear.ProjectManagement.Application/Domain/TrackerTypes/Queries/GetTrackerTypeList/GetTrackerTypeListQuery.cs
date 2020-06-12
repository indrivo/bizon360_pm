using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeList
{
    public class GetTrackerTypeListQuery:IRequest<TrackerTypeListViewModel>
    {
        public Guid? ActivityTypeId { get; set; }
    }
}
