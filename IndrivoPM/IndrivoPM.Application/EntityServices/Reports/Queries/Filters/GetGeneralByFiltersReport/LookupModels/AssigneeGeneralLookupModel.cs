using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels
{
    public class AssigneeGeneralLookupModel
    {
        public Guid AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public float TotalLoggedTime { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<AssigneeGeneralDto>>, AssigneeGeneralLookupModel>> Projection
        {
            get
            {
                return assignee => new AssigneeGeneralLookupModel
                {
                    AssigneeId = assignee.Key,
                    AssigneeName = assignee.Value[0].AssigneeName,
                    TotalLoggedTime = assignee.Value.Sum(x => x.LoggedTime)
                };
            }
        }

        public static AssigneeGeneralLookupModel Create(KeyValuePair<Guid, List<AssigneeGeneralDto>> sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
