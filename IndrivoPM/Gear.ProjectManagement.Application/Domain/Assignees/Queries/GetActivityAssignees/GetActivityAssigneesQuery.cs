using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Assignees.Queries.GetActivityAssignees
{
    public class GetActivityAssigneesQuery : IRequest<ActivityAssigneesDto>
    {
        public Guid ActivityId { get; set; }
    }
}
