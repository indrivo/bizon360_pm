using System.Collections.Generic;
using System.Linq;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.FilteredViewModels
{
    public class ProjectFilteredView
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ProjectFilteredLookupModel> Projects { get; set; }
            = new List<ProjectFilteredLookupModel>();

        public static ProjectFilteredView Create(List<ActivityDto> result)
        {
            var activities = result.GroupBy(x => x.ActivityId, dto => new ActivityDto
            {
                AssigneeName = dto.AssigneeName,
                LoggedTime = dto.LoggedTime,
                ActivityName = dto.ActivityName,
                ActivityType = dto.ActivityType,
                ActivityStatus = dto.ActivityStatus,
                EstimatedTime = dto.EstimatedTime,
                AssigneeId = dto.AssigneeId,
                CreateTime = dto.CreateTime,
                ProjectId = dto.ProjectId
            }).ToDictionary(x => x.Key, x => x.ToList());

            return new ProjectFilteredView
            {
                Projects = activities.Select(ProjectFilteredLookupModel.Create).ToList(),
                TotalLoggedTime = result.Sum(x => x.LoggedTime)
            };
        }
    }
}
