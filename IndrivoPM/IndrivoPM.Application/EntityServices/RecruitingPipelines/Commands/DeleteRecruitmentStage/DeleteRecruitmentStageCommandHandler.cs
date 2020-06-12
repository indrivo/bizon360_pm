using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitmentStage
{
    public class DeleteRecruitmentStageCommandHandler : IRequestHandler<DeleteRecruitmentStageCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteRecruitmentStageCommandHandler(IGearContext context) => _context = context;

        public async Task<Unit> Handle(DeleteRecruitmentStageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecruitmentStages.FindAsync(request.Id);
            entity.Deactivate();

            _context.RecruitmentStages.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
