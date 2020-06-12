using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels
{
    public class ProjectGroupGeneralLookupModel
    {
        public Guid ProjectGroupId { get; set; }

        public string ProjectGroupName { get; set; }

        public ProjectGeneralView ProjectView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ProjectGroupGeneralDto>>, ProjectGroupGeneralLookupModel>> Projection
        {
            get
            {
                return projectGroup => new ProjectGroupGeneralLookupModel
                {
                    ProjectGroupId = projectGroup.Key,
                    ProjectGroupName = projectGroup.Value[0].ProjectGroupName,
                    ProjectView = ProjectGeneralView.Create(projectGroup.Value)
                };
            }
        }

        public static ProjectGroupGeneralLookupModel Create(KeyValuePair<Guid, List<ProjectGroupGeneralDto>> projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
