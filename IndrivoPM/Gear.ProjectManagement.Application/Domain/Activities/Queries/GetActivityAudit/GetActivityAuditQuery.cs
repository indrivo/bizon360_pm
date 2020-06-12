using System;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityAudit
{
    public class GetActivityAuditQuery : IRequest<ActivityAuditListView>
    {
        public Guid Id { get; set; }
    }
}
