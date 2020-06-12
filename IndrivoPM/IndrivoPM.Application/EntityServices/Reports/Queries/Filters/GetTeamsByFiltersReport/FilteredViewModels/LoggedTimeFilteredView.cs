using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.FilteredViewModels
{
    public class LoggedTimeFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<LoggedTimesFilteredByTeamsLookupModel> LoggedTimes { get; set; }

        public static LoggedTimeFilteredView Create(IList<LoggedTimeDto> result)
        {
            return new LoggedTimeFilteredView
            {
                LoggedTimes = result.Take(1).Select(LoggedTimesFilteredByTeamsLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(g => g.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityStatus, t.EstimatedTime }, (t, g) => g.First()).Sum(g => g.EstimatedTime)
            };
        }
    }
}
