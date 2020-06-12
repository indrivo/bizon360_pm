using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.LookupModels;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport
{
    public class ProjectGroupFilteredReportListViewModel
    {
        public float TotalLoggedTime { get; set; }

        public Interval IntervalType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public IList<ProjectGroupFilteredLookupModel> ProjectGroups { get; set; }
            = new List<ProjectGroupFilteredLookupModel>();

        public static async Task<ProjectGroupFilteredReportListViewModel> Create(IQueryable<ProjectLogsDto> queryResult, Interval intervalType, 
            DateTime startDate, DateTime dueDate, CancellationToken cancellationToken)
        {
            var result = (await queryResult.ToListAsync(cancellationToken));

            return await Task.Run(() =>
            {
                var projectGroups = result.GroupBy(x => x.ProjectGroupId, ProjectGroupDto.Create)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new ProjectGroupFilteredReportListViewModel
                {
                    ProjectGroups = projectGroups.Select(x => ProjectGroupFilteredLookupModel.Create(x, intervalType)).ToList(),
                    TotalLoggedTime = result.Sum(x => x.LoggedTime),
                    IntervalType = intervalType,
                    StartDate = startDate,
                    DueDate = dueDate
                };
            }, cancellationToken);
        }
    }
}
