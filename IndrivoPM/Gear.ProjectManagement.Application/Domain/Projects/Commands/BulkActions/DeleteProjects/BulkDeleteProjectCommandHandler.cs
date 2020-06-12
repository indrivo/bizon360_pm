using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.DeleteProjects
{
    public class BulkDeleteProjectCommandHandler : IRequestHandler<BulkDeleteProjectCommand, Unit>
    {
        private readonly IGearContext _context;

        public BulkDeleteProjectCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BulkDeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var projectsToDelete = new List<Project>();

            foreach (var id in request.Projects)
            {
                var project = await _context.Projects.FindAsync(id);

                if (project == null)
                {
                    throw new NotFoundException(nameof(Project), id);
                }

                projectsToDelete.Add(project);
            }

            if (projectsToDelete.Any())
            {
                _context.Projects.RemoveRange(projectsToDelete);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
