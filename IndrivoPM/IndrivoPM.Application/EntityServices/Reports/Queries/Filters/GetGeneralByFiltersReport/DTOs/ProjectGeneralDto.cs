using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs
{
    public class ProjectGeneralDto
    {
        public Guid SprintId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }


        public string ProjectName { get; set; }

        public string SprintName { get; set; }

        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<ProjectGroupGeneralDto, ProjectGeneralDto>> Projection
        {
            get
            {
                return project => new ProjectGeneralDto
                {
                    SprintId = project.SprintId,
                    ActivityId = project.ActivityId,
                    AssigneeId = project.AssigneeId,
                    ProjectName = project.ProjectName,
                    SprintName = project.SprintName,
                    ActivityName = project.ActivityName,
                    AssigneeName = project.AssigneeName,
                    EstimatedTime = project.EstimatedTime,
                    LoggedTime = project.LoggedTime,
                    CreateTime = project.CreateTime
                };
            }
        }

        public static ProjectGeneralDto Create(ProjectGroupGeneralDto project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
