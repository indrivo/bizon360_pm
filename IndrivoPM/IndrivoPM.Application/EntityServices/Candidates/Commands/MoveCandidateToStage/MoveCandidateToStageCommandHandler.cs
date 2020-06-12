using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToStage
{
    public class MoveCandidateToStageCommandHandler : IRequestHandler<MoveCandidateToStageCommand>
    {
        private readonly IGearContext _context;

        public MoveCandidateToStageCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MoveCandidateToStageCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _context.Candidates.FindAsync(request.CandidateId);

            var stage = await _context.RecruitmentStages.FindAsync(request.StageId);

            //TODO: move candidate functionality to add
            
            _context.Candidates.Update(candidate);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
