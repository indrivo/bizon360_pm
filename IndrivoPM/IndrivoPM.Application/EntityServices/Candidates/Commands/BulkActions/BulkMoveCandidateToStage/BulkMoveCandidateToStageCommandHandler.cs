using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToStage
{
    public class BulkMoveCandidateToStageCommandHandler : IRequestHandler<BulkMoveCandidateToStageCommand>
    {
        private readonly IGearContext _context;

        public BulkMoveCandidateToStageCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BulkMoveCandidateToStageCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.CandidatesId)
            {
                var candidate = await _context.Candidates.FindAsync(id);
                //TODO:Correct this domain candidate.RecruitmentStageId = request.StageId;

                _context.Candidates.Update(candidate);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
