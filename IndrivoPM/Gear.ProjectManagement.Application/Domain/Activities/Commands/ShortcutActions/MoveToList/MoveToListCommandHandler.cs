using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToList
{
    public class MoveToListCommandHandler : IRequestHandler<MoveToListCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public MoveToListCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(MoveToListCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            entity.ActivityListId = request.ActivityListId;
            

            _context.Activities.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
