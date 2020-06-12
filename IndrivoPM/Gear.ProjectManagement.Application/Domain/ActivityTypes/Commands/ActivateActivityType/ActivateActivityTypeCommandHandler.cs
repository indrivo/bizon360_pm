using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.ActivateActivityType
{
    public class ActivateActivityTypeCommandHandler : IRequestHandler<ActivateActivityTypeCommand >
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public ActivateActivityTypeCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(ActivateActivityTypeCommand  request, CancellationToken cancellationToken)
        {
            Guid userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            ActivityType entity = _context.ActivityTypes.Find(request.Id);
            if(entity == null) throw new NotFoundException("Not exist an entity with this id", nameof(ActivityType));

            if (request.Active)
                entity.Activate();
              else
                entity.Deactivate();

            _context.ActivityTypes.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
