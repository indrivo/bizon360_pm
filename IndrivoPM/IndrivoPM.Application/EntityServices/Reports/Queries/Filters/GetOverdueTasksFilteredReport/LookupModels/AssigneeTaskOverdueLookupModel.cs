using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.LookupModels
{
    public class AssigneeTaskOverdueLookupModel
    {
        public Guid AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public DateTime? Deadline { get; set; }

        public int Overdue { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<AssigneeOverdueDto>>, AssigneeTaskOverdueLookupModel>> Projection
        {
            get
            {
                return assignee => new AssigneeTaskOverdueLookupModel
                {
                    AssigneeId = assignee.Key,
                    AssigneeName = assignee.Value[0].AssigneeName,
                    Deadline = assignee.Value[0].Deadline,
                    Overdue = (int)(assignee.Value[0].Deadline.HasValue ? DateTime.Now - assignee.Value[0].Deadline.Value : DateTime.Now - DateTime.MinValue).TotalDays
                };
            }
        }

        public static AssigneeTaskOverdueLookupModel Create(KeyValuePair<Guid, List<AssigneeOverdueDto>> result)
        {
            return Projection.Compile().Invoke(result);
        }
    }
}
