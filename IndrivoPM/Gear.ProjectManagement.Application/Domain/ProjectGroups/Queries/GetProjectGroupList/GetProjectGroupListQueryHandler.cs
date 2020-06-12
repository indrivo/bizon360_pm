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

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList
{
    public class GetProjectGroupListQueryHandler : IRequestHandler<GetProjectGroupListQuery, ProjectGroupListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public GetProjectGroupListQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<ProjectGroupListViewModel> Handle(GetProjectGroupListQuery request, CancellationToken cancellationToken)
        {
            var projectGroups = _context.ProjectGroups
                .Include(pg => pg.Projects)
                .OrderByDescending(pg => pg.IsDeletable).ThenBy(pg => pg.Name);

            if (request.HasAccessToAllEntities)
            {
                return new ProjectGroupListViewModel
                {
                    ProjectGroups = projectGroups.Select(pg => ProjectGroupLookupModel.Create(pg, request.Filter)).ToList()
                };
            }

            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            return new ProjectGroupListViewModel
            {
                ProjectGroups = projectGroups.Where(pg => pg.Projects.Any(p => p.ProjectMembers.Any(pm => pm.UserId == userId)))
                    .ToList().Select(pg => ProjectGroupLookupModel.Create(pg, request.Filter)).ToList()
            };
        }
    }
}
