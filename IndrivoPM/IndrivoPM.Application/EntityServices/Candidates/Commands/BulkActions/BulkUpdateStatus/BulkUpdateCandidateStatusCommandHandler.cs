using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkUpdateStatus
{
    public class BulkUpdateCandidateStatusCommandHandler : IRequestHandler<BulkUpdateCandidateStatusCommand, Unit>
    {
        private readonly IGearContext _context;

        public BulkUpdateCandidateStatusCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BulkUpdateCandidateStatusCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.Candidates)
            {
                var candidate = await _context.Candidates.FindAsync(id);

                if (candidate == null)
                {
                    throw new NotFoundException(nameof(Candidate), id);
                }

                candidate.ChangeStatus(request.Status);

                _context.Candidates.Update(candidate);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
