using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByPriorityReport
{
    public class GetTasksByPriorityReportQueryHandler : IRequestHandler<GetTasksByPriorityReportQuery, ProjectTaskStatisticListViewModel>
    {
        private readonly IGearContext _context;

        public GetTasksByPriorityReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectTaskStatisticListViewModel> Handle(GetTasksByPriorityReportQuery request, CancellationToken cancellationToken)
        {
            if (!request.ShowAllData && !request.ActivityPriorities.Any())
                throw new NotFoundException(nameof(GetTasksByPriorityReportQuery), new { request.ShowAllData, request.ActivityPriorities });

            var project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectId);

            var activities = await _context.Activities
                .Where(x => x.ProjectId == request.ProjectId 
                        && (request.ShowAllData || request.ActivityPriorities.Contains(x.ActivityPriority)))
                .GroupBy(x => x.ActivityPriority).ToListAsync(cancellationToken);

            return new ProjectTaskStatisticListViewModel
            {
                ActivityPriorities = activities
                    .ToDictionary(x => x.Key, x => x.ToList())
                    .Select(ActivityPriorityModel.Create).ToList()
            };
        }
    }
}
