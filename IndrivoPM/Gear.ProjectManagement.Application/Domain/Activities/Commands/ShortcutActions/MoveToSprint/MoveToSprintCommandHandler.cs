using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToSprint
{
    public class MoveToSprintCommandHandler : IRequestHandler<MoveToSprintCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public MoveToSprintCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(MoveToSprintCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            var sprint = await _context.Sprints.FindAsync(request.SprintId);

            entity.SprintId = request.SprintId;
            entity.DueDate = sprint.EndDate;
            

            _context.Activities.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
