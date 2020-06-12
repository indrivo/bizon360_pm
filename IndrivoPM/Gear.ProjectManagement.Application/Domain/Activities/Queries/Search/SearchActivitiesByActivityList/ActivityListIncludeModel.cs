using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByActivityList
{
    public class ActivityListIncludeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int CompletedActivitiesCount { get; set; }
        public int ActivitiesCount { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public int Progress { get; set; }

        public ICollection<ActivityLookupModel> Activities { get; set; }

        public static Expression<Func<ActivityList, ActivityListIncludeModel>> SimpleProjection
        {
            get
            {
                return activityList => new ActivityListIncludeModel
                {
                    Id = activityList.Id,
                    Name = activityList.Name,
                    CompletedActivitiesCount = activityList.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = activityList.Activities.Count,
                    EstimatedTime = activityList.Activities.Any() ? activityList.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                    LoggedTime = activityList.Activities.Any() ? activityList.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0,
                    Progress = activityList.Activities.Any() ? (int)activityList.Activities.Average(a => a.Progress) : 0,
                    Activities = activityList.Activities.Select(ActivityLookupModel.Create).ToList()
                };
            }
        }
        public static Expression<Func<ActivityList, string, int, ActivityListIncludeModel>> Projection
        {
            get
            {
                return (activityList, searchValue, number) => new ActivityListIncludeModel
                {
                    Id = activityList.Id,
                    Name = activityList.Name,
                    CompletedActivitiesCount = activityList.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = activityList.Activities.Count,
                    EstimatedTime = activityList.Activities.Any() ? activityList.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                    LoggedTime = activityList.Activities.Any() ? activityList.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0,
                    Progress = activityList.Activities.Any() ? (int)activityList.Activities.Average(a => a.Progress) : 0,
                    Activities = activityList.Activities
                        .Where(a => a.Name.ToLower().Contains(searchValue.ToLower()) || a.Number == number)
                        .Select(ActivityLookupModel.Create).ToList()
                };
            }
        }

        public static ActivityListIncludeModel Create(ActivityList activityList)
        {
            return SimpleProjection.Compile().Invoke(activityList);
        }
        public static ActivityListIncludeModel Create(ActivityList activityList, string searchValue, int number)
        {
            if (string.IsNullOrEmpty(searchValue)) return SimpleProjection.Compile().Invoke(activityList);

            return Projection.Compile().Invoke(activityList, searchValue, number);
        }
    }
}