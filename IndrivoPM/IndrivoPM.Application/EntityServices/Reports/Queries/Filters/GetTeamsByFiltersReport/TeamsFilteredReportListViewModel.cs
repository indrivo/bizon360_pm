using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.LookupModels;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport
{
    public class TeamsFilteredReportListViewModel
    {
        public float TotalLoggedTime { get; set; }

        public float TotalEstimatedTime { get; set; }

        public IList<AssigneeFilteredLookupModel> Assignees { get; set; }

        public static async Task<TeamsFilteredReportListViewModel> Create(IQueryable<AssigneeLogsDto> queryResult,
            CancellationToken cancellationToken)
        {
            var result = await queryResult.ToListAsync(cancellationToken);

            return await Task.Run(() =>
            {
                var teams = result.GroupBy(x => x.AssigneeId, dto => new ActivityDto
                {
                    LoggedTime = dto.LoggedTime,
                    ActivityName = dto.ActivityName,
                    ActivityStatus = dto.ActivityStatus,
                    EstimatedTime = dto.EstimatedTime,
                    ProjectName = dto.ProjectName,
                    LoggedTimeId = dto.LoggedTimeId,
                    AssigneeName = dto.AssigneeName,
                    ActivityId = dto.ActivityId,
                    ProjectId = dto.ProjectId,
                    CreateTime = dto.CreateTime
                }).ToDictionary(x => x.Key, x => x.ToList());

                return new TeamsFilteredReportListViewModel
                {
                    Assignees = teams.Select(AssigneeFilteredLookupModel.Create).ToList(),
                    TotalLoggedTime = result.Sum(x => x.LoggedTime),
                    TotalEstimatedTime = result.GroupBy(t => new { t.ActivityName, t.EstimatedTime }, (t, g) => g.First()).Sum(g => g.EstimatedTime)
                };
            }, cancellationToken);
        }
    }
}
