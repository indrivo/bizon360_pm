using System;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup
{
    public class GetProjectListByGroupQuery : IRequest<ProjectListViewModel>
    {
        public Guid? GroupId { get; set; }

        public ProjectStatus Status { get; set; }
    }
}
