using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers
{
    public class GetProjectActivityTypeTrackersQuery : IRequest<ProjectActivityTrackersListViewModel>
    {
        public Guid ProjectId { get; set; }

        public Guid ActivityTypeId { get; set; }
    }
}
