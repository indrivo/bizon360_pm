using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport
{
    public class ProjectGeneralReportLookupModel
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }


        public string ActivityName { get; set; }

        public string SprintName { get; set; }

        public string ActivityListName { get; set; }

        public ActivityPriority ActivityPriority { get; set; }

        public float Estimated { get; set; }

        public float Logged { get; set; }

        public int Progress { get; set; }
        
        public ActivityStatus ActivityStatus { get; set; }

        public DateTime LastModified { get; set; }

        public static Expression<Func<ActivityAssignee, ProjectGeneralReportLookupModel>> Projection
        {
            get
            {
                return activity => new ProjectGeneralReportLookupModel
                {
                    EmployeeId = activity.UserId,
                    EmployeeName = activity.User != null
                        ? $"{activity.User.FirstName} {activity.User.LastName}" : "Undefined",
                    ActivityName = activity.Activity != null ? activity.Activity.Name : "Undefined",
                    SprintName = activity.Activity != null && activity.Activity.Sprint != null
                        ? activity.Activity.Sprint.Name : "Undefined",
                    ActivityListName = activity.Activity != null && activity.Activity.ActivityList != null
                        ? activity.Activity.ActivityList.Name : "Undefined",
                    ActivityPriority = activity.Activity.ActivityPriority,
                    Estimated = activity.Activity != null ? activity.Activity.EstimatedHours ?? 0f : 0f,
                    Progress = activity.Activity != null ? activity.Activity.Progress : 0,
                    ActivityStatus = activity.Activity.ActivityStatus,
                    LastModified = activity.Activity.ModifyTime
                };
            }
        }

        public static ProjectGeneralReportLookupModel Create(ActivityAssignee activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
