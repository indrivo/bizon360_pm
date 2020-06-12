using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityList
{
    public class ActivityLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ApplicationUserLookupModel> Assignees { get; set; }

        public ActivityPriority Priority { get; set; }

        public ActivityStatus Status { get; set; }

        public int Progress { get; set; }

        public ActivityType ActivityType { get; set; }

        public float? EstimatedHours { get; set; }

        public float? LoggedTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public Guid? ActivityListId { get; set; }

        public string ActivityListName { get; set; }

        public Guid? SprintId { get; set; }

        public int Number { get; set; }

        public static Expression<Func<Activity, ActivityLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityLookupModel
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Description = activity.Description,
                    Priority = activity.ActivityPriority,
                    Status = activity.ActivityStatus,
                    Progress = activity.Progress,
                    ActivityType = activity.ActivityType,
                    EstimatedHours = activity.EstimatedHours,
                    LoggedTime = activity.LoggedTimes.Sum(lt => lt.Time),
                    StartDate = activity.StartDate,
                    DueDate = activity.DueDate,
                    ProjectId = activity.ProjectId,
                    ProjectName = activity.Project.Name,
                    ActivityListId = activity.ActivityListId,
                    ActivityListName = activity.ActivityList == null ? "None" : activity.ActivityList.Name,
                    Assignees = activity.Assignees.Select(aa => ApplicationUserLookupModel.Create(aa.User)).ToList(),
                    //Sprint = activity.Sprint
                    SprintId = activity.SprintId,
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
