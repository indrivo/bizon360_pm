using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails
{
    public class GetActivityDetailQuery : IRequest<ActivityDetailModel>
    {
        public Guid Id { get; set; }

        public bool DisplayAuditInfo { get; set; } = false;
    }
}
