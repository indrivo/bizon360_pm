using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.DeleteProjectGroup
{
    public class DeleteProjectGroupCommandHandler : IRequestHandler<DeleteProjectGroupCommand>
    {
        private readonly IGearContext _context;

        public DeleteProjectGroupCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProjectGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ProjectGroups.FindAsync(request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ProjectGroup), request.Id);
            }

            if (!entity.IsDeletable)
            {
                throw new DeleteFailureException(nameof(ProjectGroup),request.Id, DeleteFailureMessages.EntityNotDeletable.ToString());
            }

            var groupProjects = _context.Projects.Where(p => p.ProjectGroupId == entity.Id);
            if (groupProjects.Any())
            {
                var projectUpdateList = new List<Project>();
                foreach (var project in groupProjects)
                {
                    project.ProjectGroupId = _context.ProjectGroups.First(pg => !pg.IsDeletable).Id;
                    projectUpdateList.Add(project);
                }

                _context.Projects.UpdateRange(projectUpdateList);
            }

            _context.ProjectGroups.Remove(entity);
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
