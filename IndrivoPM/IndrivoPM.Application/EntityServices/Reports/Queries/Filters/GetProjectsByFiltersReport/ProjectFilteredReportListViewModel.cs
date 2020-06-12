using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.LookupModels;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport
{
    public class ProjectFilteredReportListViewModel
    {
        public float TotalEstimatedTime { get; set; }

        public float TotalLoggedTime { get; set; }

        public IList<ProjectFilteredLookupModel> Projects { get; set; } 
            = new List<ProjectFilteredLookupModel>();

        public static async Task<ProjectFilteredReportListViewModel> Create(IQueryable<ProjectLoggsDto> queryResult, CancellationToken cancellationToken)
        {
            var result = await queryResult.ToListAsync(cancellationToken);

            return await Task.Run(() =>
            {
                var projects = result.GroupBy(x => x.ProjectId, dto => new ActivityDto
                {
                    ProjectName = dto.ProjectName,
                    AssigneeName = dto.AssigneeName,
                    LoggedTime = dto.LoggedTime,
                    ActivityName = dto.ActivityName,
                    ActivityStatus = dto.ActivityStatus,
                    ActivityType = dto.ActivityType,
                    EstimatedTime = dto.EstimatedTime,
                    CreateTime = dto.CreateTime,
                    ActivityId = dto.ActivityId,
                    AssigneeId = dto.AssigneeId
                }).ToDictionary(x => x.Key, x => x.ToList());

                return new ProjectFilteredReportListViewModel
                {
                    Projects = projects.Select(ProjectFilteredLookupModel.Create).ToList(),
                    TotalLoggedTime = result.Sum(x => x.LoggedTime),
                    TotalEstimatedTime = result.GroupBy(t => new { t.ActivityName, t.EstimatedTime }, (t, g) => g.First()).Sum(x => x.EstimatedTime)
                };
            }, cancellationToken);
        }
    }
}
