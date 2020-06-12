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

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectList
{
    public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, ProjectListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetProjectListQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectListViewModel> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            if (request.GetAllProjects)
            {
                return new ProjectListViewModel
                {
                    Projects = (await _context.Projects.Include(x => x.ProjectManager)
                        .Include(p => p.Activities)
                        .Include(x => x.ProjectMembers).ThenInclude(x => x.User)
                        .Include(x => x.ProjectGroup).ToListAsync(cancellationToken)).Select(ProjectLookupModel.Create).ToList()
                };
            }
            return new ProjectListViewModel
            {
                Projects = (await _context.Projects.Include(x => x.ProjectManager)
                    .Include(p => p.Activities)
                    .Include(x => x.ProjectMembers).ThenInclude(x => x.User)
                    .Include(x => x.ProjectGroup)
                    .Where(p => p.ProjectMembers.Any(x => x.UserId == userId))
                    .ToListAsync(cancellationToken)).Select(ProjectLookupModel.Create).ToList()
            };

        }

    }
}