using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList
{
    public class GetProjectLoggedTimeReportListQueryHandler : IRequestHandler<GetProjectLoggedTimeReportListQuery, ProjectLoggedTimeReportListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectLoggedTimeReportListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectLoggedTimeReportListViewModel> Handle(GetProjectLoggedTimeReportListQuery request, CancellationToken cancellationToken)
        {
            var projects = new ProjectLoggedTimeReportListViewModel {ProjectReports = new List<ProjectLoggedTimeReportLookupModel>()};

            // Get projects with includes.
            var projectIncludes = _context.Projects
                .Include(x => x.Activities)
                .ThenInclude(x => x.LoggedTimes);

            // Filter activities from projects by start activity date and end date.
            var query = request.ProjectIds.Join(projectIncludes, pid => pid, project => project.Id,
                (pid, project) => project).ToList();

            // Mapping and calculate total estimated and logged time.
            foreach (var project in query)
            {
                project.Activities = project.Activities
                    .Where(x => x.StartDate >= request.StartDate && x.DueDate <= request.DueDate).ToList();
                var projectLookup = ProjectLoggedTimeReportLookupModel.Create(project);
                projects.ProjectReports.Add(projectLookup);
                projects.TotalEstimatedTime += projectLookup.EstimatedTime;
                projects.TotalLoggedTime += projectLookup.LoggedTime;
            }

            return await Task.FromResult(projects);
        }
    }
}
