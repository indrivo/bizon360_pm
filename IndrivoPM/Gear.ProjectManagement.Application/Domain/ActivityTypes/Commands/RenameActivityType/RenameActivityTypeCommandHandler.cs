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

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.RenameActivityType
{
    public class RenameActivityTypeCommandHandler : IRequestHandler<RenameActivityTypeCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public RenameActivityTypeCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(RenameActivityTypeCommand request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal user = _userAccessor.GetUser();

            Guid userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            ActivityType entity = await _context.ActivityTypes.FindAsync(request.Id);

            if (entity == null) throw new NotFoundException("Not exist an entity with this id", nameof(ActivityType));

            entity.Name = request.Name;
            

            _context.ActivityTypes.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
