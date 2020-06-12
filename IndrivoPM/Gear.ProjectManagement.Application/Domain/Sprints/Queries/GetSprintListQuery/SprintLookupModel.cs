using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery
{
    public class SprintLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CompletedActivitiesCount { get; set; }

        public int ActivitiesCount { get; set; }
        public int TotalActivitiesCount { get; set; }
        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public static Expression<Func<Sprint, SprintLookupModel>> SimpleProjection
        {
            get
            {
                return sprint => new SprintLookupModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    CompletedActivitiesCount = sprint.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = sprint.Activities.Count,
                    TotalActivitiesCount = sprint.Activities.Count,
                    EstimatedTime = sprint.Activities.Any() ? sprint.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                    LoggedTime = sprint.Activities.Any() ? sprint.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0,
                    DueDate = sprint.EndDate,
                    IsCompleted = sprint.SprintStatus == SprintStatus.Completed
                };
            }
        }
        public static Expression<Func<Sprint, ICollection<ActivityStatus>, SprintLookupModel>> Projection
        {
            get
            {
                return (sprint, filter) => new SprintLookupModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    CompletedActivitiesCount = sprint.Activities.Count(a => a.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = sprint.Activities.Count(a => filter.Any(tag => a.ActivityStatus == tag)),
                    TotalActivitiesCount = sprint.Activities.Count,
                    EstimatedTime = sprint.Activities.Any() ? sprint.Activities.Sum(a => a.EstimatedHours ?? 0) : 0,
                    LoggedTime = sprint.Activities.Any()
                        ? sprint.Activities.Select(a => a.LoggedTimes.Sum(lt => lt.Time)).Sum()
                        : 0,
                    DueDate = sprint.EndDate,
                    IsCompleted = sprint.SprintStatus == SprintStatus.Completed
                };
            }
        }

        public static SprintLookupModel Create(Sprint sprint)
        {
            return SimpleProjection.Compile().Invoke(sprint);
        }
        public static SprintLookupModel Create(Sprint sprint, ICollection<ActivityStatus> filter)
        {
            if (filter == null || !filter.Any()) return SimpleProjection.Compile().Invoke(sprint);

            return Projection.Compile().Invoke(sprint, filter);
        }
    }
}
