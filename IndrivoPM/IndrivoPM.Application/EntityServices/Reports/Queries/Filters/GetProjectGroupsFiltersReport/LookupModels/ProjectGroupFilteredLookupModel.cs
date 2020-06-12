using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.LookupModels
{
    public class ProjectGroupFilteredLookupModel
    {
        public Guid ProjectGroupId { get; set; }

        public string ProjectGroupName { get; set; }

        public ProjectFilteredView ProjectView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ProjectGroupDto>>, Interval, ProjectGroupFilteredLookupModel>> Projection
        {
            get
            {
                return (projectGroup, interval) => new ProjectGroupFilteredLookupModel
                {
                    ProjectGroupId = projectGroup.Key,
                    ProjectGroupName = projectGroup.Value[0].ProjectGroupName,
                    ProjectView = ProjectFilteredView.Create(projectGroup.Value, interval)
                };
            }
        }

        public static ProjectGroupFilteredLookupModel Create(KeyValuePair<Guid, List<ProjectGroupDto>> projectGroup, Interval intervalType)
        {
            return Projection.Compile().Invoke(projectGroup, intervalType);
        }
    }
}
