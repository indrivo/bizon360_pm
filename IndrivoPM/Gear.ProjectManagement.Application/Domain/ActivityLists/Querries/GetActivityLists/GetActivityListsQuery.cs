using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists
{
    public class GetActivityListsQuery : IRequest<ActivityListsViewModel>
    {
        public Guid ProjectId { get; set; }
        public ICollection<ActivityStatus> Filter { get; set; }
    }
}
