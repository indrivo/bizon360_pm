using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport
{
    public class GetTasksByTypeReportQueryHandler : IRequestHandler<GetTasksByTypeReportQuery, ProjectTaskStatisticListViewModel>
    {
        private readonly IGearContext _context;

        public GetTasksByTypeReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectTaskStatisticListViewModel> Handle(GetTasksByTypeReportQuery request, CancellationToken cancellationToken)
        {
            if (!request.ShowAllData && !request.ActivityTypes.Any())
                throw new NotFoundException(nameof(GetTasksByTypeReportQuery), new {request.ShowAllData, request.ActivityTypes});

            var project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectId);

            var filteredActivities = await _context.Activities
                .Include(x => x.ActivityType)
                .Where(x => x.ProjectId == request.ProjectId 
                       && (request.ShowAllData || request.ActivityTypes.Contains(x.ActivityTypeId)))
                .GroupBy(x => x.ActivityTypeId).ToListAsync(cancellationToken);

            var activityTypes = filteredActivities
                .ToDictionary(x => x.Key, x => x.ToList());

            return new ProjectTaskStatisticListViewModel
            {
                ProjectId = request.ProjectId,
                ActivityTypes = activityTypes.Select(ActivityTypeModel.Create).ToList()
            };
        }
    }
}
