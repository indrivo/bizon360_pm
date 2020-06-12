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

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.RenameSprintCommand
{
    public class RenameSprintCommandHandler : IRequestHandler<RenameSprintCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public RenameSprintCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(RenameSprintCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = await _context.Sprints.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Sprint), request.Id);
            }

            entity.Name = request.Name;
            

            _context.Sprints.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
