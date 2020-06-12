using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels
{
    public class ActivityGeneralLookupModel
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public float EstimatedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public AssigneeGeneralView AssigneeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityGeneralDto>>, ActivityGeneralLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityGeneralLookupModel
                {
                    ActivityId = activity.Key,
                    ActivityName = activity.Value[0].ActivityName,
                    CreateTime = activity.Value[0].CreateTime,
                    EstimatedTime = activity.Value[0].EstimatedTime,
                    AssigneeView = AssigneeGeneralView.Create(activity.Value)
                };
            }
        }

        public static ActivityGeneralLookupModel Create(KeyValuePair<Guid, List<ActivityGeneralDto>> sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
