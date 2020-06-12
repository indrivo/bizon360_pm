using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.FilteredViewModels
{
    public class ActivityFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ActivityFilteredByTeamsLookupModel> Activities { get; set; }

        public static ActivityFilteredView Create(List<ActivityDto> result)
        {
            var assignees = result.GroupBy(x => x.ActivityId, dto => new LoggedTimeDto
            {
                LoggedTime = dto.LoggedTime,
                EstimatedTime = dto.EstimatedTime,
                ActivityStatus = dto.ActivityStatus,
                ProjectName = dto.ProjectName,
                LoggedTimeId = dto.LoggedTimeId,
                ActivityName = dto.ActivityName,
                ProjectId = dto.ProjectId,
                CreateTime = dto.CreateTime
            }).ToDictionary(x => x.Key, x => x.ToList());

            return new ActivityFilteredView
            {
                Activities = assignees.Select(ActivityFilteredByTeamsLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(g => g.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.ActivityName, t.EstimatedTime }, (t, g) => g.First()).Sum(g => g.EstimatedTime)
            };
        }
    }
}
