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

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.RenameActivityList
{
    public class RenameActivityListCommandHandler : IRequestHandler<RenameActivityListCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public RenameActivityListCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(RenameActivityListCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = await _context.ActivityLists.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ActivityList), request.Id);
            }

            entity.Name = request.Name;
            

            _context.ActivityLists.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
