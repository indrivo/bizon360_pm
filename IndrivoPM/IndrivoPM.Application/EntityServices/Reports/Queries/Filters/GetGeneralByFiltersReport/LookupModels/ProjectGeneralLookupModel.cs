using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels
{
    public class ProjectGeneralLookupModel
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public SprintGeneralView SprintView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<ProjectGeneralDto>>, ProjectGeneralLookupModel>> Projection
        {
            get
            {
                return project => new ProjectGeneralLookupModel
                {
                    ProjectId = project.Key,
                    ProjectName = project.Value[0].ProjectName,
                    SprintView = SprintGeneralView.Create(project.Value)
                };
            }
        }

        public static ProjectGeneralLookupModel Create(KeyValuePair<Guid, List<ProjectGeneralDto>> projectGroup)
        {
            return Projection.Compile().Invoke(projectGroup);
        }
    }
}
