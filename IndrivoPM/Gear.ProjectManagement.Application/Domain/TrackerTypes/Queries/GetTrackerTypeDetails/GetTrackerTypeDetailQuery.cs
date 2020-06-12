using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeDetails
{
    public class GetTrackerTypeDetailQuery:IRequest<TrackerTypeDetailModel>
    {
        public Guid Id { get; set; }

    }
}
