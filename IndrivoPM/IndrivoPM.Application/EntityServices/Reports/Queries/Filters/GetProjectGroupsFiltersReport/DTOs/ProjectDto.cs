using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs
{
    public class ProjectDto
    {
        public Guid AssigneeId { get; set; }

        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime Date { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<ProjectGroupDto, ProjectDto>> Projection
        {
            get
            {
                return project => new ProjectDto
                {
                    AssigneeId = project.AssigneeId,
                    ProjectName = project.ProjectName,
                    AssigneeName = project.AssigneeName,
                    Date = project.Date,
                    LoggedTime = project.LoggedTime
                };
            }
        }

        public static ProjectDto Create(ProjectGroupDto projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
