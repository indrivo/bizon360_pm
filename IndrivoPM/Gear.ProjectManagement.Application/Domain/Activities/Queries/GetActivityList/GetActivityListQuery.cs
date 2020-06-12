using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityList
{
    public class GetActivityListQuery : IRequest<ActivitiesListViewModel>
    {
        public Guid ProjectId { get; set; }

        public bool UserIsInvolved { get; set; }
    }
}
