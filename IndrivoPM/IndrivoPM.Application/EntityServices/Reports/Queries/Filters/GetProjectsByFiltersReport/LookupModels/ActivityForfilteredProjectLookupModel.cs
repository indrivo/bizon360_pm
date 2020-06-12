using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels
{
    public class ActivityForFilteredProjectLookupModel
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public DateTime CreateTime { get; set; }

        public AssigneeFilteredView AssigneeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<AssigneeDto>>, ActivityForFilteredProjectLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityForFilteredProjectLookupModel
                {
                    ActivityId = activity.Key,
                    ActivityName = activity.Value[0].ActivityName,
                    CreateTime = activity.Value[0].CreateTime,
                    AssigneeView = AssigneeFilteredView.Create(activity.Value)
                };
            }
        }

        public static ActivityForFilteredProjectLookupModel Create(KeyValuePair<Guid, List<AssigneeDto>> result)
        {
            return Projection.Compile().Invoke(result);
        }
    }
}
