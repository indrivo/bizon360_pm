using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.LookupModels
{
    public class ActivityFilteredByEmployeeLookupModel
    {
        public string ActivityName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public string ProjectName { get; set; }

        public float EstimatedTime { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ProjectId { get; set; }

        public DateTime CreateTime { get; set; }

        public LoggedTimeByEmployeeFilteredView LoggedTimeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityFilteredDto>>, ActivityFilteredByEmployeeLookupModel>> Projection
        {
            get
            {
                return activity => new ActivityFilteredByEmployeeLookupModel
                {
                    ActivityName = activity.Value[0].ActivityName,
                    ActivityStatus = activity.Value[0].ActivityStatus,
                    ProjectName = activity.Value[0].ProjectName,
                    EstimatedTime = activity.Value[0].EstimatedTime,
                    ActivityId = activity.Value[0].ActivityPkId,
                    ProjectId = activity.Value[0].ProjectId,
                    CreateTime = activity.Value[0].CreateTime,
                    LoggedTimeView = LoggedTimeByEmployeeFilteredView.Create(activity.Value)
                };
            }
        }

        public static ActivityFilteredByEmployeeLookupModel Create(KeyValuePair<Guid, List<ActivityFilteredDto>> activity)
        {
            return Projection.Compile().Invoke(activity);
        }
    }
}
