using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class GetProjectGroupsWithProjectsQueryHandler : IRequestHandler<GetProjectGroupsWithProjectsQuery, ProjectGroupCollection>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetProjectGroupsWithProjectsQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectGroupCollection> Handle(GetProjectGroupsWithProjectsQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            if (request.GetAllProjects)
            {
                return new ProjectGroupCollection
                {
                    ProjectGroups = (await _context.ProjectGroups
                            .Include(pg => pg.Projects).ThenInclude(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                            .Include(pg => pg.Projects).ThenInclude(p => p.Activities)
                            .OrderByDescending(pg => pg.IsDeletable).ThenBy(pg => pg.Name)
                            .ToListAsync(cancellationToken))
                        .Select(ProjectGroupLookupModel.Create).ToList()
                };
            }

            return new ProjectGroupCollection
            {
                ProjectGroups = (await _context.ProjectGroups
                        .Include(pg => pg.Projects).ThenInclude(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                        .Include(pg => pg.Projects).ThenInclude(p => p.Activities)
                        .Where(pg => pg.Projects.Any(p => p.ProjectMembers.Any(pm => pm.UserId == userId)))
                        .OrderByDescending(pg => pg.IsDeletable).ThenBy(pg => pg.Name)
                        .ToListAsync(cancellationToken))
                    .Select(pg => ProjectGroupLookupModel.Create(pg, userId)).ToList()
            };
        }
    }
}
