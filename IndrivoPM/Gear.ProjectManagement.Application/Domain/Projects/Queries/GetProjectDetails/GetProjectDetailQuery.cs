using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails
{
    public class GetProjectDetailQuery : IRequest<ProjectDetailModel>
    {
        public Guid Id { get; set; }
    }
}