using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs
{
    public class LoggedTimeDto
    {
        public DateTime Date { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<AssigneeDto, LoggedTimeDto>> Projection
        {
            get
            {
                return assignee => new LoggedTimeDto
                {
                    Date = assignee.Date,
                    LoggedTime = assignee.LoggedTime
                };
            }
        }

        public static LoggedTimeDto Create(AssigneeDto assignee)
        {
            return Projection.Compile().Invoke(assignee);
        }
    }
}
