using System;
using System.Collections.Generic;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup
{
    public class ProjectListViewModel
    {
        public Guid ProjectGroupId { get; set; }

        public int ProjectsCount { get; set; }

        public ICollection<ProjectLookupModel> Projects { get; set; }
    }
}