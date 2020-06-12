using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.LookupModels;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport
{
    public class GeneralFilteredReportListViewModel
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<ProjectGroupGeneralLookupModel> ProjectGroups { get; set; }

        public static async Task<GeneralFilteredReportListViewModel> Create(IQueryable<ProjectGroupLogsDto> queryResult,
            CancellationToken cancellationToken)
        {
            var result = await queryResult.ToListAsync(cancellationToken);

            return await Task.Run(() =>
            {
                var projectGroups = result.GroupBy(x => x.AssigneeId, ProjectGroupGeneralDto.Create)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new GeneralFilteredReportListViewModel
                {
                    ProjectGroups = projectGroups.Select(ProjectGroupGeneralLookupModel.Create).ToList(),
                    TotalLoggedTime = result.Sum(x => x.LoggedTime),
                    TotalEstimatedTime = result.GroupBy(t => new { t.ActivityId, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
                };
            }, cancellationToken);
        }
    }
}
