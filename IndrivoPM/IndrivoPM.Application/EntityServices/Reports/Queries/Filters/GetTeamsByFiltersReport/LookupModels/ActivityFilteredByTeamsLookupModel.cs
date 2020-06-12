using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.LookupModels
{
    public class ActivityFilteredByTeamsLookupModel
    {
        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid ProjectId { get; set; }

        public LoggedTimeFilteredView LoggedTimeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<LoggedTimeDto>>, ActivityFilteredByTeamsLookupModel>> Projection
        {
            get
            {
                return project => new ActivityFilteredByTeamsLookupModel
                {
                    ActivityId = project.Key,
                    ActivityName = project.Value[0].ActivityName,
                    ProjectId = project.Value[0].ProjectId,
                    CreateTime = project.Value[0].CreateTime,
                    LoggedTimeView = LoggedTimeFilteredView.Create(project.Value)
                };
            }
        }

        public static ActivityFilteredByTeamsLookupModel Create(KeyValuePair<Guid, List<LoggedTimeDto>> project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
