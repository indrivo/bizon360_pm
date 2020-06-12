using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.LookupModels;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport
{
    public class OverdueTasksFilteredReportViewModel
    {
        public IList<ProjectGroupTaskOverdueLookupModel> ProjectGroups { get; set; }

        public static async Task<OverdueTasksFilteredReportViewModel> Create(IQueryable<OverdueResultDto> queryResult, CancellationToken cancellationToken)
        {
            var result = await queryResult.ToListAsync(cancellationToken);

            return await Task.Run(() =>
            {
                var projectGroups = result.GroupBy(x => x.ProjectGroupId, ProjectGroupOverdueDto.Create)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new OverdueTasksFilteredReportViewModel
                {
                    ProjectGroups = projectGroups.Select(ProjectGroupTaskOverdueLookupModel.Create).ToList()
                };
            }, cancellationToken);
        }
    }
}
