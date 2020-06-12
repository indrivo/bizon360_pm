using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class ProjectGroupLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Progress { get; set; }

        public ICollection<ProjectLookupModel> Projects { get; set; }

        public bool IsDeletable { get; set; }

        public static Expression<Func<ProjectGroup, ProjectGroupLookupModel>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupLookupModel
                {
                    Id = projectGroup.Id,
                    Name = projectGroup.Name,
                    Progress = projectGroup.Projects.Any() ? (int) projectGroup.Projects.Average(p => p.Activities.Any() ? (int) p.Activities.Average(a => a.Progress) : 0) : 0,
                    Projects = projectGroup.Projects.Select(ProjectLookupModel.Create).ToList(),
                    IsDeletable = projectGroup.IsDeletable
                };
            }
        }

        public static ProjectGroupLookupModel Create(ProjectGroup projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }

        public static ProjectGroupLookupModel Create(ProjectGroup projectGroup, Guid? userId)
        {
            if (userId.HasValue && !userId.Equals(Guid.Empty))
                projectGroup.Projects = projectGroup.Projects
                    .Where(p => p.ProjectMembers.Any(pm => pm.UserId == userId.Value)).ToList();

            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
