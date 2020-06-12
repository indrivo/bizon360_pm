using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.FilteredViewModels
{
    public class ActivityByEmployeeFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ActivityFilteredByEmployeeLookupModel> Activities { get; set; }

        public static ActivityByEmployeeFilteredView Create(List<AssigneeFilteredDto> result)
        {

            var projects = result.GroupBy(x => x.ActivityId, ActivityFilteredDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new ActivityByEmployeeFilteredView
            {
                Activities = projects.Select(ActivityFilteredByEmployeeLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityId, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
            };
        }
    }
}
