using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs
{
    public class ProjectOverdueDto
    {
        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }

        public string ProjectName { get; set; }

        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime? Deadline { get; set; }

        public static Expression<Func<ProjectGroupOverdueDto, ProjectOverdueDto>> Projection
        {
            get
            {
                return projectGroup => new ProjectOverdueDto
                {
                    ActivityId = projectGroup.ActivityId,
                    ActivityName = projectGroup.ActivityName,
                    AssigneeId = projectGroup.AssigneeId,
                    ProjectName = projectGroup.ProjectName,
                    AssigneeName = projectGroup.AssigneeName,
                    Deadline = projectGroup.Deadline
                };
            }
        }

        public static ProjectOverdueDto Create(ProjectGroupOverdueDto overdue)
        {
            return Projection.Compile().Invoke(overdue);
        }
    }
}
