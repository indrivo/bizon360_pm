using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesBySprint
{
    public class SprintIncludeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int CompletedActivitiesCount { get; set; }
        public int ActivitiesCount { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public int Progress { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public ICollection<ActivityLookupModel> Activities { get; set; }

        public static Expression<Func<Sprint, SprintIncludeModel>> Projection
        {
            get
            {
                return sprint => new SprintIncludeModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    CompletedActivitiesCount = sprint.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = sprint.Activities.Count,
                    EstimatedTime = sprint.Activities.Any() ? sprint.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                    LoggedTime = sprint.Activities.Any() ? sprint.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0,
                    Progress = sprint.Activities.Any() ? (int)sprint.Activities.Average(a => a.Progress) : 0,
                    Activities = sprint.Activities.Select(ActivityLookupModel.Create).ToList(),
                    DueDate = sprint.EndDate,
                    IsCompleted = sprint.Activities.Any() && sprint.Activities.All(a => a.ActivityStatus == ActivityStatus.Completed)
                };
            }
        }

        public static SprintIncludeModel Create(Sprint sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
