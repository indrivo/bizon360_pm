using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.FilteredViewModels
{
    public class LoggedTimeFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public Dictionary<int, float> LoggedTimes { get; set; } = new Dictionary<int, float>();

        public static LoggedTimeFilteredView Create(IList<AssigneeDto> result, Interval intervalType)
        {
            var loggedTimes = result
                .GroupBy(x => (int)Math.Ceiling((12 * x.Date.Year + x.Date.Month) / ((float) intervalType)),
                    LoggedTimeDto.Create)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Sum(value => value.LoggedTime));

            return new LoggedTimeFilteredView
            {
                LoggedTimes = loggedTimes,
                TotalLoggedTime = result.Sum(x => x.LoggedTime)
            };
        }
    }
}
