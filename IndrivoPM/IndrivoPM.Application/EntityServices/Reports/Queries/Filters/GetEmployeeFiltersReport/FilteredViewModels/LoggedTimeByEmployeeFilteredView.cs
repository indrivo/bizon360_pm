using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.FilteredViewModels
{
    public class LoggedTimeByEmployeeFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<LoggedTimesFilteredByEmployeeLookupModel> LoggedTimes { get; set; }

        public static LoggedTimeByEmployeeFilteredView Create(List<ActivityFilteredDto> result)
        {

            var loggedTimes = result.GroupBy(x => x.LoggedTimeId, LoggedTimeFilteredDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new LoggedTimeByEmployeeFilteredView
            {
                LoggedTimes = loggedTimes.Select(LoggedTimesFilteredByEmployeeLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime)
            };
        }
    }
}
