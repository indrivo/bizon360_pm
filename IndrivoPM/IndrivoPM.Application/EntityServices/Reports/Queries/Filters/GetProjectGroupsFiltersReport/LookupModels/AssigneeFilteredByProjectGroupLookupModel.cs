using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.LookupModels
{
    public class AssigneeFilteredByProjectGroupLookupModel
    {
        public Guid AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public LoggedTimeFilteredView LoggedTimeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<AssigneeDto>>, Interval, AssigneeFilteredByProjectGroupLookupModel>> Projection
        {
            get
            {
                return (assignee, interval) => new AssigneeFilteredByProjectGroupLookupModel
                {
                    AssigneeId = assignee.Key,
                    AssigneeName = assignee.Value[0].AssigneeName,
                    LoggedTimeView = LoggedTimeFilteredView.Create(assignee.Value, interval)
                };
            }
        }

        public static AssigneeFilteredByProjectGroupLookupModel Create(KeyValuePair<Guid, List<AssigneeDto>> result, Interval intervalType)
        {
            return Projection.Compile().Invoke(result, intervalType);
        }
    }
}
