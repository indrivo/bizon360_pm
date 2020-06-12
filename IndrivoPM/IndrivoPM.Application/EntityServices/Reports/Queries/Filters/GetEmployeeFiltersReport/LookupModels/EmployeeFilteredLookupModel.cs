using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.LookupModels
{
    public class EmployeeFilteredLookupModel
    {
        public Guid AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public ActivityByEmployeeFilteredView ActivityView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<AssigneeFilteredDto>>, EmployeeFilteredLookupModel>> Projection
        {
            get
            {
                return activity => new EmployeeFilteredLookupModel
                {
                    AssigneeId = activity.Key,
                    AssigneeName = activity.Value[0].AssigneeName,
                    ActivityView = ActivityByEmployeeFilteredView.Create(activity.Value)
                };
            }
        }

        public static EmployeeFilteredLookupModel Create(KeyValuePair<Guid, List<AssigneeFilteredDto>> assignee)
        {
            return Projection.Compile().Invoke(assignee);
        }
    }
}
