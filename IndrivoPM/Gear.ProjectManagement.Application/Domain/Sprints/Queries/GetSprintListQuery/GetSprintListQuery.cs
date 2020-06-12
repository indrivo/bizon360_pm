using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery
{
    public class GetSprintListQuery : IRequest<SprintListViewModel>
    {
        public Guid ProjectId { get; set; }
        public ICollection<ActivityStatus> Filter { get; set; }
    }
}
