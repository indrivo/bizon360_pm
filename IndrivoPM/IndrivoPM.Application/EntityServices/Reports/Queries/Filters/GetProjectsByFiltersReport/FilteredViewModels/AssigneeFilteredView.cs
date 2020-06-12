using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.FilteredViewModels
{
    public class AssigneeFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ProjectsFilteredAssigneeLookupModel> Assignees { get; set; }

        public static AssigneeFilteredView Create(List<AssigneeDto> result)
        {
            var loggedTimes = result.GroupBy(x => x.AssigneeId, dto => new LoggedTimeDto
            {
                LoggedTime = dto.LoggedTime,
                ActivityType = dto.ActivityType,
                ActivityStatus = dto.ActivityStatus,
                EstimatedTime = dto.EstimatedTime,
                AssigneeName = dto.AssigneeName,
                CreateTime = dto.CreateTime
            }).ToDictionary(x => x.Key, x => x.ToList());

            return new AssigneeFilteredView
            {
                Assignees = loggedTimes.Select(ProjectsFilteredAssigneeLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityStatus, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
            };
        }
    }
}
