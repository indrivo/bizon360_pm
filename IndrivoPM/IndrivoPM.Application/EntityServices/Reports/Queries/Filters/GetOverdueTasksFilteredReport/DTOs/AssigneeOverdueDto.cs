using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs
{
    public class AssigneeOverdueDto
    {
        public string AssigneeName { get; set; }

        public DateTime? Deadline { get; set; }

        public static Expression<Func<ActivityOverdueDto, AssigneeOverdueDto>> Projection
        {
            get
            {
                return assignee => new AssigneeOverdueDto
                {
                    AssigneeName = assignee.AssigneeName,
                    Deadline = assignee.Deadline
                };
            }
        }

        public static AssigneeOverdueDto Create(ActivityOverdueDto overdue)
        {
            return Projection.Compile().Invoke(overdue);
        }
    }
}
