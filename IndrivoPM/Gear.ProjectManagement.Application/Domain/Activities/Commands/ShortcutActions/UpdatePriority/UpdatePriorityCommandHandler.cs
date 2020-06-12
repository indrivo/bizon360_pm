using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdatePriority
{
    public class UpdatePriorityCommandHandler : IRequestHandler<UpdatePriorityCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public UpdatePriorityCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdatePriorityCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Activities.FindAsync(request.Id);

            entity.ActivityPriority = request.Priority;
            

            _context.Activities.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
