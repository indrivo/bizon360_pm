using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs
{
    public class ProjectGroupGeneralDto
    {
        public Guid ProjectId { get; set; }

        public Guid SprintId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }


        public string ProjectGroupName { get; set; }

        public string ProjectName { get; set; }

        public string SprintName { get; set; }

        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<ProjectGroupLogsDto, ProjectGroupGeneralDto>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupGeneralDto
                {
                    ProjectId = projectGroup.ProjectId,
                    SprintId = projectGroup.SprintId,
                    ActivityId = projectGroup.ActivityId,
                    AssigneeId = projectGroup.AssigneeId,
                    ProjectGroupName = projectGroup.ProjectGroupName,
                    ProjectName = projectGroup.ProjectName,
                    SprintName = projectGroup.SprintName,
                    ActivityName = projectGroup.ActivityName,
                    AssigneeName = projectGroup.AssigneeName,
                    EstimatedTime = projectGroup.EstimatedTime,
                    LoggedTime = projectGroup.LoggedTime,
                    CreateTime = projectGroup.CreateTime
                };
            }
        }

        public static ProjectGroupGeneralDto Create(ProjectGroupLogsDto projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
