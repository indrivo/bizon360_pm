using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs
{
    public class ActivityFilteredDto
    {
        public string ActivityName { get; set; }

        public string ProjectName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public float LoggedTime { get; set; }

        public float EstimatedTime { get; set; }

        public Guid LoggedTimeId { get; set; }

        public Guid ActivityPkId { get; set; }

        public Guid ProjectId { get; set; }

        public DateTime CreateTime { get; set; }

        public static Expression<Func<AssigneeFilteredDto, ActivityFilteredDto>> Projection
        {
            get
            {
                return activity => new ActivityFilteredDto
                {
                    ActivityName = activity.ActivityName,
                    ProjectName = activity.ProjectName,
                    ActivityStatus = activity.ActivityStatus,
                    LoggedTime = activity.LoggedTime,
                    EstimatedTime = activity.EstimatedTime,
                    LoggedTimeId = activity.LoggedTimeId,
                    ActivityPkId = activity.ActivityPkId,
                    ProjectId = activity.ProjectId,
                    CreateTime = activity.CreateTime
                };
            }
        }

        public static ActivityFilteredDto Create(AssigneeFilteredDto activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
