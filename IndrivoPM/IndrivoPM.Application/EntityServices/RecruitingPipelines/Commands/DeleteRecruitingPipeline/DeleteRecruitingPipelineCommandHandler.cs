using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitingPipeline
{
    public class DeleteRecruitingPipelineCommandHandler : IRequestHandler<DeleteRecruitingPipelineCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteRecruitingPipelineCommandHandler(IGearContext context) => _context = context;

        public async Task<Unit> Handle(DeleteRecruitingPipelineCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecruitingPipelines.FindAsync(request.Id);
            entity.Deactivate();

            _context.RecruitingPipelines.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
