using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs
{
    public class LoggedTimeFilteredDto
    {
        public float LoggedTime { get; set; }

        public static Expression<Func<ActivityFilteredDto, LoggedTimeFilteredDto>> Projection
        {
            get
            {
                return activity => new LoggedTimeFilteredDto
                {
                    LoggedTime = activity.LoggedTime
                };
            }
        }

        public static LoggedTimeFilteredDto Create(ActivityFilteredDto activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
