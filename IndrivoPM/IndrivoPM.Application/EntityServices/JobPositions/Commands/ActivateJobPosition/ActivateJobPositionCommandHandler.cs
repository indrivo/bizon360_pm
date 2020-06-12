using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.ActivateJobPosition
{
    public class ActivateJobPositionCommandHandler : IRequestHandler<ActivateJobPositionCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public ActivateJobPositionCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(ActivateJobPositionCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids == null || request.Ids.Count == 0) throw new NullReferenceException("Invalid id");

            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            foreach (var item in request.Ids)
            {
                var entity = await _context.JobPositions.FindAsync(item);

                if (entity == null) throw new NullReferenceException("Invalid id");

                if (request.Active)
                    entity.Activate();
                else
                    entity.Deactivate();

                _context.JobPositions.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
