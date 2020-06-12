using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.MoveStageToPipeline
{
    public class MoveStageToPipelineCommandHandler : IRequestHandler<MoveStageToPipelineCommand>
    {
        private readonly IGearContext _context;

        public MoveStageToPipelineCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MoveStageToPipelineCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.StagesId)
            {
                var stage = await _context.RecruitmentStages.FindAsync(id);
                
                //TODO: change pipeline use case
                //stage.PipelineId = request.RecruitingPipelineId;

                _context.RecruitmentStages.Update(stage);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
