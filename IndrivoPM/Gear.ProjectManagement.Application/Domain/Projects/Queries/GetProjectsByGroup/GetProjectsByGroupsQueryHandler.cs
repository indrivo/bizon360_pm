using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsByGroup
{
    public class GetProjectsByGroupsQueryHandler : IRequestHandler<GetProjectsByGroupsQuery, ProjectsDto>
    {
        private readonly IGearContext _context;

        public GetProjectsByGroupsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ProjectsDto> Handle(GetProjectsByGroupsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _context.Projects.AsNoTracking()
                .Where(p => request.GroupIds.Contains(p.ProjectGroupId))
                .Select(x => new ProjectDto {Id = x.Id, Name = x.Name, ProjectGroupId = x.ProjectGroupId})
                .ToListAsync(cancellationToken: cancellationToken);

            return new ProjectsDto
            {
                Projects = projects
            };

        }
    }
}
