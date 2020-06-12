using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.DateTimeExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport
{
    public class GetProjectGroupsGeneralReportQueryHandler : IRequestHandler<GetProjectGroupsGeneralReportQuery, ProjectGroupsGeneralReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectGroupsGeneralReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectGroupsGeneralReportListViewModel> Handle(GetProjectGroupsGeneralReportQuery request, CancellationToken cancellationToken)
        {
            var result = new ProjectGroupsGeneralReportListViewModel();

            var projectGroupIds = await _context.ProjectGroups
                .Select(x => x.Id).ToListAsync();

            foreach (var projectGroupId in projectGroupIds)
            {
                var projectGroup = await _context.ProjectGroups.FindAsync(projectGroupId);

                if (projectGroup != null)
                {
                    var group = new ProjectGroupsGeneralReportLookupModel
                    {
                        ProjectGroupName = projectGroup.Name
                    };

                    var projects = await _context.Projects
                        .Where(x => x.ProjectGroupId == projectGroupId)
                        .Select(x => new ProjectReportLookupModel
                        {
                            ProjectId = x.Id,
                            ProjectName = x.Name
                        }).ToListAsync(cancellationToken);

                    foreach(var project in projects)
                    {
                        var projectActivities = _context.Activities
                            .Include(x => x.LoggedTimes)
                            .Where(x => x.ProjectId == project.ProjectId);

                        project.EstimatedTime = await projectActivities.SumAsync(x => x.EstimatedHours ?? 0f);
                        project.LoggedTime = await projectActivities.SumAsync(x => x.LoggedTimes
                            .Where(xx => xx.DateOfWork.IsInRangeInclusive(request.StartDate, request.DueDate))
                            .Sum(xx => xx.Time));

                        group.Projects.Add(project);
                        group.TotalEstimatedTime += project.EstimatedTime;
                        group.TotalLoggedTime += project.LoggedTime;
                    }

                    result.ProjectGroups.Add(group);
                }
            }

            return result;
        }
    }
}
