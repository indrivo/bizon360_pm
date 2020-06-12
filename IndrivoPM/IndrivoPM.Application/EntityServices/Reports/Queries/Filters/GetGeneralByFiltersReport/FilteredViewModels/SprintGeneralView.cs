using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels
{
    public class SprintGeneralView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<SprintGeneralLookupModel> Sprints { get; set; }

        public static SprintGeneralView Create(List<ProjectGeneralDto> result)
        {

            var projects = result.GroupBy(x => x.SprintId, SprintGeneralDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new SprintGeneralView
            {
                Sprints = projects.Select(SprintGeneralLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityId, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
            };
        }
    }
}
