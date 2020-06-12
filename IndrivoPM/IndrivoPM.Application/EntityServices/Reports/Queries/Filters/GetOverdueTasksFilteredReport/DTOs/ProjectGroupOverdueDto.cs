using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs
{
    public class ProjectGroupOverdueDto
    {
        public Guid ProjectId { get; set; }

        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public Guid AssigneeId { get; set; }

        public string ProjectGroupName { get; set; }

        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime? Deadline { get; set; }

        public static Expression<Func<OverdueResultDto, ProjectGroupOverdueDto>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupOverdueDto
                {
                    ProjectId = projectGroup.ProjectId,
                    ActivityId = projectGroup.ActivityId,
                    ActivityName = projectGroup.ActivityName,
                    AssigneeId = projectGroup.AssigneeId,
                    ProjectGroupName = projectGroup.ProjectGroupName,
                    ProjectName = projectGroup.ProjectName,
                    AssigneeName = projectGroup.AssigneeName,
                    Deadline = projectGroup.Deadline
                };
            }
        }

        public static ProjectGroupOverdueDto Create(OverdueResultDto overdue)
        {
            return Projection.Compile().Invoke(overdue);
        }
    }
}
