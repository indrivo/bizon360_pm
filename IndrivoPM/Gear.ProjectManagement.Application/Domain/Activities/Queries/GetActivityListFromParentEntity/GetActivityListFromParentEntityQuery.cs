using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity
{
    public class GetActivityListFromParentEntityQuery : IRequest<ActivityListViewModel>
    {
        public Guid ParentEntityId { get; set; }

        public ActivityParentType ParentType { get; set; }

        public int Skip { get; set; }

        public int Size { get; set; } = 10;

        public Guid ProjectId { get; set; }
        public ICollection<ActivityStatus> Filter { get; set; }
    }
}