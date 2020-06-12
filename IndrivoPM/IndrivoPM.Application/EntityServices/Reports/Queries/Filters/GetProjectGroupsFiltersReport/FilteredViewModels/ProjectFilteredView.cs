using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.FilteredViewModels
{
    public class ProjectFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public IList<ProjectFilteredByProjectGroupLookupModel> Projects { get; set; }
            = new List<ProjectFilteredByProjectGroupLookupModel>();

        public static ProjectFilteredView Create(List<ProjectGroupDto> result, Interval intervalType)
        {

            var projects = result.GroupBy(x => x.ProjectId, ProjectDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new ProjectFilteredView
            {
                Projects = projects.Select(x => ProjectFilteredByProjectGroupLookupModel.Create(x, intervalType)).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime)
            };
        }
    }
}
