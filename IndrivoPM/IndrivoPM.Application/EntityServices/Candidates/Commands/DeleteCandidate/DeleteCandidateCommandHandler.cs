using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.DeleteCandidate
{
    public class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand>
    {
        private readonly IGearContext _context;

        public DeleteCandidateCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Candidates.FindAsync(request.CandidateId);

            entity.Deactivate();
            _context.Candidates.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
