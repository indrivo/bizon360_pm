using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.PmEntities.Settings;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails
{
    public class ActivityDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Guid> Assignee { get; set; }

        public ICollection<ApplicationUserLookupModel> Assignees { get; set; }

        public ActivityPriority Priority { get; set; }

        public ActivityStatus Status { get; set; }

        public int Progress { get; set; }

        public Guid ActivityTypeId { get; set; }

        public string ActivityTypeName { get; set; }

        public float? EstimatedHours { get; set; }

        public float? LoggedTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid? ActivityListId { get; set; }

        public string ActivityListName { get; set; }

        public Guid? SprintId { get; set; }
        public Guid? ChangeRequestId { get; set; }
        public string SprintName { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int ProjectNumber { get; set; }

        public Guid? ProjectManagerId { get; set; }

        public ApplicationUserLookupModel Author { get; set; }

        public ProjectSettings Settings { get; set; }

        public int Number { get; set; }

        public IList<ActivityAuditModel> AuditChanges { get; set; } = null;


        public static Expression<Func<Activity, ActivityDetailModel>> Projection
        {
            get
            {
                return activity => new ActivityDetailModel
                {
                    Id = activity.Id,
                    Name = activity.Name,
                    Description = activity.Description,
                    Priority = activity.ActivityPriority,
                    Status = activity.ActivityStatus,
                    Progress = activity.Progress,
                    ActivityTypeId = activity.ActivityTypeId,
                    ActivityTypeName = activity.ActivityType.Name,
                    EstimatedHours = activity.EstimatedHours,
                    DueDate = activity.DueDate,
                    ActivityListId = activity.ActivityListId,
                    ActivityListName = activity.ActivityList == null ? "None" : activity.ActivityList.Name,
                    SprintId = activity.SprintId,
                    ChangeRequestId = activity.ChnageRequestId,
                    SprintName = activity.Sprint != null ? activity.Sprint.Name : string.Empty,
                    StartDate = activity.StartDate,
                    ProjectId = activity.ProjectId,
                    ProjectName = activity.Project.Name,
                    ProjectManagerId = activity.Project.ProjectManagerId,
                    Assignees = activity.Assignees.Select(aa => ApplicationUserLookupModel.Create(aa.User)).ToList(),
                    Assignee = activity.Assignees.Select(aa => aa.UserId).ToList(),
                    LoggedTime = activity.LoggedTimes.Sum(x => x.Time),
                    Settings = activity.Project.ProjectSettings,
                    Number = activity.Number,
                    ProjectNumber = activity.Project.Number ?? 0
                };
            }
        }

        public static ActivityDetailModel Create(Activity activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
