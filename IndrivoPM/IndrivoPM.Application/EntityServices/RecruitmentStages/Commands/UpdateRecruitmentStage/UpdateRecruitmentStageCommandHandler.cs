using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.UpdateRecruitmentStage
{
    public class UpdateRecruitmentStageCommandHandler : IRequestHandler<UpdateRecruitmentStageCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public UpdateRecruitmentStageCommandHandler(IMediator mediator,
            IGearContext context,
            IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateRecruitmentStageCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.RecruitmentStages.FindAsync(request.Id);
            entity.Name = request.Name;

            //TODO: Check for pipeline switch use case
            //entity.PipelineId = request.RecruitingPipelineId;

            _context.RecruitmentStages.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new RecruitmentStageUpdated
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            };

            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
