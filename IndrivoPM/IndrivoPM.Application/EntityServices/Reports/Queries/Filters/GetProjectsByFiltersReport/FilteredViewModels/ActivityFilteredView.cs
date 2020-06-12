using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.FilteredViewModels
{
    public class ActivityFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ActivityForFilteredProjectLookupModel> Activities { get; set; }
            = new List<ActivityForFilteredProjectLookupModel>();

        public static ActivityFilteredView Create(List<ActivityDto> result)
        {
            var assignees = result.GroupBy(x => x.ActivityId, dto => new AssigneeDto
            {
                LoggedTime = dto.LoggedTime,
                EstimatedTime = dto.EstimatedTime,
                AssigneeName = dto.AssigneeName,
                ActivityType = dto.ActivityType,
                ActivityStatus = dto.ActivityStatus,
                ActivityName = dto.ActivityName,
                AssigneeId = dto.AssigneeId,
                CreateTime = dto.CreateTime
            }).ToDictionary(x => x.Key, x => x.ToList());

            return new ActivityFilteredView
            {
                Activities = assignees.Select(ActivityForFilteredProjectLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime),
                TotalEstimatedTime = result.GroupBy(t => new { t.AssigneeName, t.EstimatedTime }, (t, g) => g.First().EstimatedTime).Sum()
            };
        }
    }
}
