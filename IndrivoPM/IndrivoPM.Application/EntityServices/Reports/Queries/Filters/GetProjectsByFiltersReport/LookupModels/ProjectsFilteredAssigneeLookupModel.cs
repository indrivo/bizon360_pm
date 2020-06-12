using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels
{
    public class ProjectsFilteredAssigneeLookupModel
    {
        public Guid AssigneeId { get; set; }

        public string AssigneeName { get; set; }

        public LoggedTimeFilteredView LoggedTimeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<LoggedTimeDto>>, ProjectsFilteredAssigneeLookupModel>> Projection
        {
            get
            {
                return assignee => new ProjectsFilteredAssigneeLookupModel
                {
                    AssigneeId = assignee.Key,
                    AssigneeName = assignee.Value[0].AssigneeName,
                    LoggedTimeView = LoggedTimeFilteredView.Create(assignee.Value)
                };
            }
        }

        public static ProjectsFilteredAssigneeLookupModel Create(KeyValuePair<Guid, List<LoggedTimeDto>> result)
        {
            return Projection.Compile().Invoke(result);
        }
    }
}
