using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.DeleteHeadline
{
    public class DeleteHeadlineCommandHandler : IRequestHandler<DeleteHeadlineCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteHeadlineCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteHeadlineCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.WikiHeadlines.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Headline), request.Id);
            }

            _context.WikiHeadlines.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
