using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.LookupModels
{
    public class ProjectFilteredByProjectGroupLookupModel
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public AssigneeFilteredView AssigneeView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ProjectDto>>, Interval, ProjectFilteredByProjectGroupLookupModel>> Projection
        {
            get
            {
                return (project, interval) => new ProjectFilteredByProjectGroupLookupModel
                {
                    ProjectId = project.Key,
                    ProjectName = project.Value[0].ProjectName,
                    AssigneeView = AssigneeFilteredView.Create(project.Value, interval)
                };
            }
        }

        public static ProjectFilteredByProjectGroupLookupModel Create(KeyValuePair<Guid, List<ProjectDto>> result, Interval intervalType)
        {
            return Projection.Compile().Invoke(result, intervalType);
        }
    }
}
