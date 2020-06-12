using System;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetPersonalActivitiesByProject
{
    public class GetPersonalActivitiesByProjectQuery : IRequest<ActivityListViewModel>
    {
        public Guid UserId { get; set; }
    }
}
