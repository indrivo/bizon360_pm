using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsByGroup;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsTeams
{
    public class GetProjectsTeamsQueryHandler : IRequestHandler<GetProjectsTeamsQuery, TeamsDto>
    {
        private readonly IGearContext _context;

        public GetProjectsTeamsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<TeamsDto> Handle(GetProjectsTeamsQuery request, CancellationToken cancellationToken)
        {
            var users = _context.ApplicationUsers.AsNoTracking()
                .Where(u => request.ProjectsIds.HasProject(u.Projects))
                .ToList();

            return await Task.FromResult(new TeamsDto
            {
                Users = users.Select(user => new UserDto
                {
                    Id = user.Id, 
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                }).ToList()
            });
        }
        
    }
    public static class UserExtension
    {
        public static bool HasProject(this List<Guid> projectsIds, ICollection<ProjectMember> userProjects)
        {
            return userProjects.Any(up => projectsIds.Contains(up.ProjectId));
        }
    }
}

