using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels
{
    public class AssigneeGeneralView
    {
        public float TotalLoggedTime { get; set; }

        public IList<AssigneeGeneralLookupModel> Assignees { get; set; }

        public static AssigneeGeneralView Create(List<ActivityGeneralDto> result)
        {

            var assignees = result.GroupBy(x => x.AssigneeId, AssigneeGeneralDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return new AssigneeGeneralView
            {
                Assignees = assignees.Select(AssigneeGeneralLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime)
            };
        }
    }
}
