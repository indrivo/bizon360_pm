using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintDetailQuery
{
    public class GetSprintDetailQuery : IRequest<SprintDetailModel>
    {
        public Guid Id { get; set; }
    }
}
