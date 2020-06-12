using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.CreateTrackerType
{
    public class CreateTrackerTypeCommandHandler : IRequestHandler<CreateTrackerTypeCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public CreateTrackerTypeCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(CreateTrackerTypeCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = new TrackerType
            {
                ActivityTypeId = request.ActivityTypeId,
            };

            entity.CreateEnd(Guid.NewGuid(), request.Name, userId);

            if (request.Active)
                entity.Activate();
            else
                entity.Deactivate();

            _context.TrackerTypes.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new TrackerTypeCreated
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
