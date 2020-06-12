using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByEmployeesReport
{
    public class EmployeeModel
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int ActivitiesCount { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityAssignee>>, EmployeeModel>> Projection
        {
            get
            {
                return activity => new EmployeeModel
                {
                    EmployeeId = activity.Key,
                    EmployeeName = $"{activity.Value[0].User.FirstName} {activity.Value[0].User.LastName}",
                    ActivitiesCount = activity.Value.Count
                };
            }
        }

        public static EmployeeModel Create(KeyValuePair<Guid, List<ActivityAssignee>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
