using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels
{
    public class ProjectFilteredLookupModel
    {
        public string ProjectName { get; set; }

        public Guid ProjectId { get; set; }

        public ActivityFilteredView ActivityView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ActivityDto>>, ProjectFilteredLookupModel>> Projection
        {
            get
            {
                return project => new ProjectFilteredLookupModel
                {
                    ProjectId = project.Key,
                    ProjectName = project.Value[0].ProjectName,
                    ActivityView = ActivityFilteredView.Create(project.Value)
                };
            }
        }

        public static ProjectFilteredLookupModel Create(KeyValuePair<Guid, List<ActivityDto>> project)
        {
            return Projection.Compile().Invoke(project);
        }
    }
}
