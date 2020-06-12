using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup
{
    public class GetProjectListByGroupQueryHandler : IRequestHandler<GetProjectListByGroupQuery, ProjectListViewModel>
    {
        private readonly IGearContext _context;

        public GetProjectListByGroupQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectListViewModel> Handle(GetProjectListByGroupQuery request, CancellationToken cancellationToken)
        {
            if (!request.GroupId.HasValue || request.GroupId == Guid.Empty)
            {
                request.GroupId = _context.ProjectGroups.First(pg => !pg.IsDeletable).Id;
            }

            var projects = (await _context.Projects
                    .Include(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                    .Include(p => p.Activities).ToListAsync(cancellationToken))
                .Where(p => p.ProjectGroupId == request.GroupId && (p.Status == ProjectStatus.New || p.Status == ProjectStatus.InProgress))
                .Select(ProjectLookupModel.Create).ToList();

            return new ProjectListViewModel
            {
                ProjectGroupId = request.GroupId ?? Guid.Empty,
                ProjectsCount = projects.Count,
                Projects = projects
            };
        }
    }
}
