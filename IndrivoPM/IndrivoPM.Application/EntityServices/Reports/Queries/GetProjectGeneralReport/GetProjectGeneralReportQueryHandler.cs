using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport
{
    public class GetProjectGeneralReportQueryHandler : IRequestHandler<GetProjectGeneralReportQuery, ProjectGeneralReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectGeneralReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectGeneralReportListViewModel> Handle(GetProjectGeneralReportQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId);
            }

            var result = new ProjectGeneralReportListViewModel
            {
                ProjectName = project.Name
            };

            var projectActivityIds = await _context.Activities
                .Where(a => a.ProjectId == request.ProjectId)
                .Select(x => x.Id).ToListAsync();

            foreach(var activityId in projectActivityIds)
            {
                var activities = (await _context.ActivityAssignees
                    .Where(a => a.ActivityId == activityId)
                    .Include(x => x.Activity).ThenInclude(xx => xx.ActivityList)
                    .Include(x => x.Activity).ThenInclude(xx => xx.Sprint)
                    .Include(x => x.User).ToListAsync(cancellationToken))
                    .Select(ProjectGeneralReportLookupModel.Create).ToList();

                foreach (var activity in activities)
                {
                    activity.Logged = await _context.LoggedTimes.Where(x => x.ActivityId == activityId
                        && x.UserId == activity.EmployeeId).SumAsync(t => t.Time);

                    result.TotalEstimatedTime += activity.Estimated;
                    result.TotalLoggedTime += activity.Logged;

                    result.Activities.Add(activity);
                }
            }

            return result;
        }
    }
}
