using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.LookupModels
{
    public class ActivityTaskOverdueLookupModel
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public IList<AssigneeTaskOverdueLookupModel> Assignees { get; set; }

        public static IList<AssigneeTaskOverdueLookupModel> SetAssignees(List<ActivityOverdueDto> result)
        {
            var activities = result.GroupBy(x => x.AssigneeId, AssigneeOverdueDto.Create)
                .ToDictionary(x => x.Key, x => x.ToList());

            return activities.Select(AssigneeTaskOverdueLookupModel.Create).ToList();
        }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityOverdueDto>>, ActivityTaskOverdueLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityTaskOverdueLookupModel
                {
                    ActivityId = activity.Key,
                    ActivityName = activity.Value[0].ActivityName,
                    Assignees = ActivityTaskOverdueLookupModel.SetAssignees(activity.Value)
                };
            }
        }

        public static ActivityTaskOverdueLookupModel Create(KeyValuePair<Guid, List<ActivityOverdueDto>> projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
