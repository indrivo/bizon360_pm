using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs
{
    public class ActivityOverdueDto
    {
        public Guid AssigneeId { get; set; }

        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime? Deadline { get; set; }

        public static Expression<Func<ProjectOverdueDto, ActivityOverdueDto>> Projection
        {
            get
            {
                return projectGroup => new ActivityOverdueDto
                {
                    AssigneeId = projectGroup.AssigneeId,
                    ActivityName = projectGroup.ActivityName,
                    AssigneeName = projectGroup.AssigneeName,
                    Deadline = projectGroup.Deadline
                };
            }
        }

        public static ActivityOverdueDto Create(ProjectOverdueDto overdue)
        {
            return Projection.Compile().Invoke(overdue);
        }
    }
}
