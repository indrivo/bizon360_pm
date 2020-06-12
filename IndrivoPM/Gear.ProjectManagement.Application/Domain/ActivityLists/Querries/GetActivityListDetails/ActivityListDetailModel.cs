using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListDetails
{
    public class ActivityListDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ActivityListStatus ActivityListStatus { get; set; }

        public Guid? SprintId { get; set; }

        public string SprintName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string Description { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int Number { get; set; }

        public int ProjectNumber { get; set; }

        public static Expression<Func<ActivityList, ActivityListDetailModel>> Projection
        {
            get
            {
                return activityList => new ActivityListDetailModel
                {
                    Id = activityList.Id,
                    Name = activityList.Name,
                    ActivityListStatus = activityList.ActivityListStatus,
                    SprintId = activityList.SprintId,
                    SprintName = activityList.Sprint != null
                        ? activityList.Sprint.Name
                        : string.Empty,
                    Description = activityList.Description,
                    StartDate = activityList.StartDate,
                    DueDate = activityList.DueDate,
                    ProjectId = activityList.ProjectId,
                    ProjectName = activityList.Project != null 
                        ? activityList.Project.Name 
                        : string.Empty,
                    Number = activityList.Number,
                    ProjectNumber = activityList.Project.Number ?? 0
                };
            }
        }

        public static ActivityListDetailModel Create(ActivityList activityList)
        {
            return Projection.Compile().Invoke(activityList);
        }
    }
}
