using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity
{
    public class ActivityLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ActivityPriority Priority { get; set; }

        public string ActivityType { get; set; }

        public ColorBadge ActivityTypeColor { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public int Progress { get; set; }

        public ICollection<ApplicationUserLookupModel> Assignees { get; set; }

        public Guid? SprintId { get; set; }

        public string Sprint { get; set; }

        public DateTime? SprintDueDate { get; set; }

        public bool SprintIsCompleted { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public ActivityStatus Status { get; set; }

        public Guid? ActivityListId { get; set; }

        public int Number { get; set; }

        public static Expression<Func<Activity, ActivityLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityLookupModel
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Priority = activity.ActivityPriority,
                    ActivityType = activity.ActivityType.Abbreviation,
                    ActivityTypeColor = activity.ActivityType.ColorBadge,
                    EstimatedTime = activity.EstimatedHours ?? 0,
                    LoggedTime = activity.LoggedTimes.Sum(lt => lt.Time),
                    Progress = activity.Progress,
                    Assignees = activity.Assignees.Select(aa => ApplicationUserLookupModel.Create(aa.User)).ToList(),
                    SprintId = activity.SprintId,
                    Sprint = activity.Sprint != null ? activity.Sprint.Name : string.Empty,
                    SprintDueDate = activity.Sprint != null ? activity.Sprint.EndDate : DateTime.MinValue,
                    SprintIsCompleted = activity.Sprint != null && activity.Sprint.Activities.All(a => a.ActivityStatus == ActivityStatus.Completed),
                    DueDate = activity.DueDate,
                    ProjectId = activity.ProjectId,
                    ProjectName = activity.Project != null ? activity.Project.Name : string.Empty,
                    Status = activity.ActivityStatus,
                    ActivityListId = activity.ActivityListId,
                    Number = activity.Number
                };
            }
        }

        public static ActivityLookupModel Create(Activity activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
