using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.LookupModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport
{
    public class EmployeeFilteredReportListViewModel
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<EmployeeFilteredLookupModel> Assignees { get; set; }

        public static async Task<EmployeeFilteredReportListViewModel> Create(IQueryable<AssigneeLogsFilteredDto> queryResult,
            CancellationToken cancellationToken)
        {
            var result = await queryResult.ToListAsync(cancellationToken);

            return await Task.Run(() =>
            {
                var projectGroups = result.GroupBy(x => x.AssigneeId, AssigneeFilteredDto.Create)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new EmployeeFilteredReportListViewModel
                {
                    Assignees = projectGroups.Select(EmployeeFilteredLookupModel.Create).ToList(),
                    TotalLoggedTime = result.Sum(x => x.LoggedTime),
                    TotalEstimatedTime = result.GroupBy(t => new { t.ActivityId, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
                };
            }, cancellationToken);
        }
    }
}
