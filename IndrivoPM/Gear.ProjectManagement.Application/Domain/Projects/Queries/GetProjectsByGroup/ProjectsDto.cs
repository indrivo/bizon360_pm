using System;
using System.Collections.Generic;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsByGroup
{
    public class ProjectsDto
    {
        public ICollection<ProjectDto> Projects { get; set; }
    }
}