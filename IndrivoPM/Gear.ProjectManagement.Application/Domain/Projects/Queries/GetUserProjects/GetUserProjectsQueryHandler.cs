using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetAllProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetUserProjects
{
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, ProjectsDto>
    {
        private readonly IGearContext _context;

        public GetUserProjectsQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<ProjectsDto> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await (from pm in _context.ProjectMembers
                join u in _context.ApplicationUsers on pm.UserId equals u.Id
                join p in _context.Projects on pm.ProjectId equals p.Id
                where u.Id == request.UserId
                select new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToListAsync(cancellationToken: cancellationToken);

            return new ProjectsDto
            {
                Projects = projects
            };
        }
    }
}
