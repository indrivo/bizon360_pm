using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.UpdateRecruitingPipeline
{
    public class UpdateRecruitingPipelineCommandHandler : IRequestHandler<UpdateRecruitingPipelineCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public UpdateRecruitingPipelineCommandHandler(IMediator mediator,
            IGearContext context,
            IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateRecruitingPipelineCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.RecruitingPipelines.FindAsync(request.Id);
            
            entity.Name = request.Name;
            entity.Description = request.Description;

            _context.RecruitingPipelines.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new RecruitingPipelineUpdated
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
