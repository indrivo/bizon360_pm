using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByGroup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectListByStatus
{
    public class GetProjectListByStatusQueryHandler : IRequestHandler<GetProjectListByStatusQuery, ProjectListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetProjectListByStatusQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectListViewModel> Handle(GetProjectListByStatusQuery request, CancellationToken cancellationToken)
        {
            if (!request.Filter.Any())
            {
                request.Filter.Add(ProjectStatus.New);
                request.Filter.Add(ProjectStatus.InProgress);
            }

            var query = (await _context.Projects
                    .Include(p => p.ProjectMembers).ThenInclude(pm => pm.User)
                    .Include(p => p.Activities).ToListAsync(cancellationToken))
                .Where(p => request.Filter.Any(tag => p.Status == tag));

            if (!request.GetAllProjects)
            {
                var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);
                query = query.Where(p => p.ProjectMembers.Any(pm => pm.UserId == userId));
            }

            var projects = query.Select(ProjectLookupModel.Create).ToList();

            return new ProjectListViewModel
            {
                Projects = projects,
                ProjectGroupId = Guid.Empty,
                ProjectsCount = projects.Count
            };
        }
    }
}
