using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.LookupModels
{
    public class AssigneeFilteredLookupModel
    {
        public Guid AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public ActivityFilteredView ActivityView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityDto>>, AssigneeFilteredLookupModel>> Projection
        {
            get
            {
                return assignee => new AssigneeFilteredLookupModel
                {
                    AssigneeId = assignee.Key,
                    AssigneeName = assignee.Value[0].AssigneeName,
                    ActivityView = ActivityFilteredView.Create(assignee.Value)
                };
            }
        }

        public static AssigneeFilteredLookupModel Create(KeyValuePair<Guid, List<ActivityDto>> project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
