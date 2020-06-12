using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList
{
    public class ProjectGroupLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CompletedProjectsCount { get; set; }

        public int ProjectsCount { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public bool IsDeletable { get; set; }

        public static Expression<Func<ProjectGroup, ProjectGroupLookupModel>> SimpleProjection
        {
            get
            {
                return projectGroup => new ProjectGroupLookupModel
                {
                    Id = projectGroup.Id,
                    Name = projectGroup.Name,
                    CompletedProjectsCount = projectGroup.Projects.Count(p => p.Status == ProjectStatus.Completed),
                    ProjectsCount = projectGroup.Projects.Count,
                    IsDeletable = projectGroup.IsDeletable
                };
            }
        }

        public static Expression<Func<ProjectGroup, ICollection<ProjectStatus>, ProjectGroupLookupModel>> Projection
        {
            get
            {
                return (projectGroup, filter) => new ProjectGroupLookupModel
                {
                    Id = projectGroup.Id,
                    Name = projectGroup.Name,
                    CompletedProjectsCount = projectGroup.Projects.Count(p => p.Status == ProjectStatus.Completed),
                    ProjectsCount = projectGroup.Projects.Count(p => filter.Any(tag => p.Status == tag)),
                    IsDeletable = projectGroup.IsDeletable
                };
            }
        }

        public static ProjectGroupLookupModel Create(ProjectGroup projectGroup)
        {
            return SimpleProjection.Compile().Invoke(projectGroup);
        }

        public static ProjectGroupLookupModel Create(ProjectGroup projectGroup, ICollection<ProjectStatus> filter)
        {
            if (filter == null || !filter.Any()) return SimpleProjection.Compile().Invoke(projectGroup);

            return Projection.Compile().Invoke(projectGroup, filter);
        }
    }
}
