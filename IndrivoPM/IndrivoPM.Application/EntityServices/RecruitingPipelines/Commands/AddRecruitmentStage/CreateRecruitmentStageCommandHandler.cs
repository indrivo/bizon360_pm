using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.AddRecruitmentStage
{
    public class CreateRecruitmentStageCommandHandler : IRequestHandler<CreateRecruitmentStageCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CreateRecruitmentStageCommandHandler(IMediator mediator,
            IGearContext context,
            IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateRecruitmentStageCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var pipeline = await _context.RecruitingPipelines.FindAsync(request.RecruitingPipelineId);
            var operation = pipeline.AddStage(request.Name);

            if (operation.IsFailure)
            {
                throw new InvalidOperationException(operation.Error);
            }

            _context.RecruitingPipelines.Update(pipeline);
            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new RecruitmentStageCreated
            {
                PrimaryEntityId = pipeline.Id,
                PrimaryEntityName = pipeline.Name,
                UserName = user.Identity.Name
            };
            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
