using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectName
{
    public class GetProjectNameQueryHandler : IRequestHandler<GetProjectNameQuery, string>
    {
        private readonly IGearContext _context;

        public GetProjectNameQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetProjectNameQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.Id);

            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            return project.Name;
        }
    }
}