using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQueryHandler: IRequestHandler<GetAllProjectsQuery,ProjectsDto>
    {
        private readonly IGearContext _context;

        public GetAllProjectsQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<ProjectsDto> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _context.Projects.AsNoTracking()
                .Select(x => new ProjectDto {Id = x.Id, Name = x.Name})
                .ToListAsync(cancellationToken: cancellationToken);

            return new ProjectsDto
            {
                Projects = projects
            };
        }
    }
}
