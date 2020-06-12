using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Settings;

namespace Gear.ProjectManagement.Manager.Domain.Projects
{
    public class ProjectTabModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ProjectManagerId { get; set; }

        public DateTime? EndDate { get; set; }

        public ProjectSettings Settings { get; set; }

        public int ProjectNumber { get; set; }

        public static Expression<Func<Project, ProjectTabModel>> Projection
        {
            get
            {
                return project => new ProjectTabModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    EndDate = project.EndDate,
                    ProjectManagerId = project.ProjectManagerId,
                    Settings = project.ProjectSettings,
                    ProjectNumber = project.Number ?? 0
                };
            }
        }

        public static ProjectTabModel Create(Project project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
