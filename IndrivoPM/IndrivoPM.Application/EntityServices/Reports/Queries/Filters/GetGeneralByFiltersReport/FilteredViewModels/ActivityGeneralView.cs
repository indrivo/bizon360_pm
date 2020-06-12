using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels
{
    public class ActivityGeneralView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ActivityGeneralLookupModel> Activities { get; set; }

        public static ActivityGeneralView Create(List<SprintGeneralDto> result)
        {

            var activities = result.GroupBy(x => x.ActivityId, ActivityGeneralDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new ActivityGeneralView
            {
                Activities = activities.Select(ActivityGeneralLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityId, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
            };
        }
    }
}
