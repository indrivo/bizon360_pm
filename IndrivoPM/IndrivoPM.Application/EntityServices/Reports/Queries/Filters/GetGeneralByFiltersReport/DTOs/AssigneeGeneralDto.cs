using System;
using System.Linq.Expressions;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs
{
    public class AssigneeGeneralDto
    {
        public string AssigneeName { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<ActivityGeneralDto, AssigneeGeneralDto>> Projection
        {
            get
            {
                return assignee => new AssigneeGeneralDto
                {
                    AssigneeName = assignee.AssigneeName,
                    LoggedTime = assignee.LoggedTime
                };
            }
        }

        public static AssigneeGeneralDto Create(ActivityGeneralDto assignee)
        {
            return Projection.Compile().Invoke(assignee);
        }
    }
}
