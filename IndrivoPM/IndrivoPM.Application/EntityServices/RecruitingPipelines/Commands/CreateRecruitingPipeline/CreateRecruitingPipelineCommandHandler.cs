using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.CreateRecruitingPipeline
{
    public class CreateRecruitingPipelineCommandHandler : IRequestHandler<CreateRecruitingPipelineCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CreateRecruitingPipelineCommandHandler(IGearContext context, 
            IMediator mediator, 
            IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(CreateRecruitingPipelineCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = new RecruitmentPipeline()
            {
                Name = request.Name,
                Description = request.Description
            };

            await _context.RecruitingPipelines.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new RecruitingPipelineCreated
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
