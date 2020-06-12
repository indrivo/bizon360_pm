using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport
{
    public class SprintGeneralLookupModel
    {
        public Guid SprintId { get; set; }

        public string SprintName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public float TotalLoggedTime { get; set; } = 0f;

        public float TotalEstimatedTime { get; set; } = 0f;

        public List<ActivityOfSprintLookupModel> Activities { get; set; }
            = new List<ActivityOfSprintLookupModel>();

        public static Expression<Func<Sprint, SprintGeneralLookupModel>> Projection
        {
            get
            {
                return sprint => new SprintGeneralLookupModel
                {
                    SprintId = sprint.Id,
                    SprintName = sprint.Name,
                    StartDate = sprint.StartDate,
                    DueDate = sprint.EndDate,
                    TotalLoggedTime = sprint.Activities.Any() 
                        ? sprint.Activities
                            .Select(a => a.LoggedTimes != null ? a.LoggedTimes.Sum(lt => lt.Time) : 0f).Sum()
                        : 0f,
                    TotalEstimatedTime = sprint.Activities.Sum(x => x.EstimatedHours ?? 0f),
                    Activities = sprint.Activities.Select(ActivityOfSprintLookupModel.Create).ToList()
                };
            }
        }
        public static SprintGeneralLookupModel Create(Sprint sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
