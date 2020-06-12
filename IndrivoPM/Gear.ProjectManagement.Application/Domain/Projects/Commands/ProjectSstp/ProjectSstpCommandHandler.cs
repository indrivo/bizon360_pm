using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ProjectSstp
{
    internal class ProjectSstpCommandHandler : IRequestHandler<ProjectSstpCommand>
    {
        private readonly IGearContext _context;

        public ProjectSstpCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ProjectSstpCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Projects.FindAsync(request.ProjectId);

            if(entity == null) throw new NotFoundException(nameof(Project), request.ProjectId);

            entity.ProductTypeId = request.ProductTypeId;
            entity.ServiceTypeId = request.ServiceTypeId;
            entity.ServiceTypeId = request.ServiceTypeId;
            entity.SolutionTypeId = request.SolutionTypeId;

            _context.Projects.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
