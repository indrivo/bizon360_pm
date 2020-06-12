using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectOverallProgress
{
    public class GetProjectOverallProgressQueryHandler : IRequestHandler<GetProjectOverallProgressQuery, ProjectOverallView>
    {
        private readonly IGearContext _context;

        public GetProjectOverallProgressQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectOverallView> Handle(GetProjectOverallProgressQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.ProjectId);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectId);

            return new ProjectOverallView
            {
                CompletedTasks = await _context.Activities
                    .Where(x => x.ProjectId == request.ProjectId
                                && x.ActivityStatus == ActivityStatus.Completed)
                    .CountAsync(cancellationToken),
                RefusedTasks = await _context.Activities
                    .Where(x => x.ProjectId == request.ProjectId
                                && x.ActivityStatus == ActivityStatus.Refused)
                    .CountAsync(cancellationToken),
                TotalTasks = await _context.Activities
                    .Where(x => x.ProjectId == request.ProjectId)
                    .CountAsync(cancellationToken),
                DaysFromStartOfProject = project.StartDate.HasValue 
                    ? (DateTime.Now - project.StartDate.Value).Days
                    : -1
            };
        }
    }
}
