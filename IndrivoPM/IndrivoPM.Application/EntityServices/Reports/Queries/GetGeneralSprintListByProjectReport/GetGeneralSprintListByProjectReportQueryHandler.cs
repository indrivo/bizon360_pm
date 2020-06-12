using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport
{
    public class GetGeneralSprintListByProjectReportQueryHandler : IRequestHandler<GetGeneralSprintListByProjectReportQuery, SprintListGeneralReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetGeneralSprintListByProjectReportQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<SprintListGeneralReportListViewModel> Handle(GetGeneralSprintListByProjectReportQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.Id);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.Id);

            var sprints = _context.Sprints
                .Where(x => x.ProjectId == request.Id)
                .Include(x => x.Activities)
                .ThenInclude(xx => xx.LoggedTimes)
                .Include(x => x.Activities)
                .ThenInclude(xx => xx.Assignees)
                .ThenInclude(xxx => xxx.User)
                .Select(SprintGeneralLookupModel.Create)
                .ToList();

            var result = new SprintListGeneralReportListViewModel
            {
                ProjectName = project.Name,
                Sprints = sprints,
                TotalEstimatedTime = sprints.Sum(x => x.TotalEstimatedTime),
                TotalLoggedTime = sprints.Sum(x => x.TotalLoggedTime)
            };

            return result;
        }
    }
}
