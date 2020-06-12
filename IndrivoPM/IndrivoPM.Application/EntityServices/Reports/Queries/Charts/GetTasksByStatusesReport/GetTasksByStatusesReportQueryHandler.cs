using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByStatusesReport
{
    public class GetTasksByStatusesReportQueryHandler : IRequestHandler<GetTasksByStatusesReportQuery, ProjectTaskStatisticListViewModel>
    {
        private readonly IGearContext _context;

        public GetTasksByStatusesReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectTaskStatisticListViewModel> Handle(GetTasksByStatusesReportQuery request, CancellationToken cancellationToken)
        {
            if (!request.ShowAllData && !request.Statuses.Any())
                throw new NotFoundException(nameof(GetTasksByStatusesReportQuery), new { request.ShowAllData, request.Statuses });

            var project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectId);

            var activities = await _context.Activities
                .Where(x => x.ProjectId == request.ProjectId
                            && (request.ShowAllData || request.Statuses.Contains(x.ActivityStatus)))
                .GroupBy(x => x.ActivityStatus)
                .ToListAsync(cancellationToken);

            return new ProjectTaskStatisticListViewModel
            {
                ActivityStatuses = activities.ToDictionary(x => x.Key, x => x.ToList())
                    .Select(ActivityStatusModel.Create).ToList()
            };
        }
    }
}
