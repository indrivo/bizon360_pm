using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.FilteredViewModels
{
    public class AssigneeFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public IList<AssigneeFilteredByProjectGroupLookupModel> Assignees { get; set; }
            = new List<AssigneeFilteredByProjectGroupLookupModel>();

        public static AssigneeFilteredView Create(List<ProjectDto> result, Interval intervalType)
        {
            var assignees = result.GroupBy(x => x.AssigneeId, AssigneeDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new AssigneeFilteredView
            {
                Assignees = assignees.Select(x => AssigneeFilteredByProjectGroupLookupModel.Create(x, intervalType)).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime)
            };
        }
    }
}
