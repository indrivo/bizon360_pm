using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateTrackerType
{
    public class UpdateTrackerTypeCommandHandler : IRequestHandler<UpdateTrackerTypeCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;
        public UpdateTrackerTypeCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(UpdateTrackerTypeCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _context.TrackerTypes.Find(request.Id);

            entity.Name = request.Name;
            entity.ActivityTypeId = request.ActivityTypeId;

            if (request.Active)
                entity.Activate();
            else
                entity.Deactivate();

            _context.TrackerTypes.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            await _mediator.Publish(new TrackerTypeUpdated
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name,
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
