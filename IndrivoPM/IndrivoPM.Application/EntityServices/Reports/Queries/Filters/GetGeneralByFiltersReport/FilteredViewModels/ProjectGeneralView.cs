using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels
{
    public class ProjectGeneralView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ProjectGeneralLookupModel> Projects { get; set; }

        public static ProjectGeneralView Create(List<ProjectGroupGeneralDto> result)
        {

            var projects = result.GroupBy(x => x.ProjectId, ProjectGeneralDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new ProjectGeneralView
            {
                Projects = projects.Select(ProjectGeneralLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityId, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
            };
        }
    }
}
