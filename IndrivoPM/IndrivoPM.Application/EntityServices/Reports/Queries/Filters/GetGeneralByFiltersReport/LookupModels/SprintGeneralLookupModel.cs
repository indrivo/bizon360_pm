using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.FilteredViewModels;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels
{
    public class SprintGeneralLookupModel
    {
        public Guid SprintId { get; set; }

        public string SprintName { get; set; }

        public ActivityGeneralView ActivityView { get; set; }

        public static Expression<Func<KeyValuePair<Guid, List<SprintGeneralDto>>, SprintGeneralLookupModel>> Projection
        {
            get
            {
                return sprint => new SprintGeneralLookupModel
                {
                    SprintId = sprint.Key,
                    SprintName = sprint.Value[0].SprintName,
                    ActivityView = ActivityGeneralView.Create(sprint.Value)
                };
            }
        }

        public static SprintGeneralLookupModel Create(KeyValuePair<Guid, List<SprintGeneralDto>> sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
