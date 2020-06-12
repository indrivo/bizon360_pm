using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivitiesForGrid
{
    public class GetActivitiesForGridQuery : IRequest<ActivitiesForGrid>
    {
        public Guid ProjectId { get; set; }

        public IList<Guid> SprintIds { get; set; }

        public bool UserIsInvolved { get; set; }
    }
}
