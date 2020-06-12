using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupDetails
{
    public class ProjectGroupDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<ProjectGroup, ProjectGroupDetailModel>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupDetailModel
                {
                    Id = projectGroup.Id,
                    Name = projectGroup.Name
                };
            }
        }

        public static ProjectGroupDetailModel Create(ProjectGroup projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
