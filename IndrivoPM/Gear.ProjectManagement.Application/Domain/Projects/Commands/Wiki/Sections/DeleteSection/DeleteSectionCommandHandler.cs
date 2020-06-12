using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.DeleteSection
{
    public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteSectionCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Sections.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Section), request.Id);
            }

            _context.Sections.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
