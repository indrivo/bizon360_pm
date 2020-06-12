using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSstp
{
    public class GetProjectSstpQuery : IRequest<ProjectSstpModel>
    {
        public Guid ProjectId { get; set; }
    }
}
