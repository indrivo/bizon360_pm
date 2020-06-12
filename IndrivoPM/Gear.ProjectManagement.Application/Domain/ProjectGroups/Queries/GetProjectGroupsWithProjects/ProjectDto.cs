using System;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Guid ProjectGroupId { get; set; }
    }
}
