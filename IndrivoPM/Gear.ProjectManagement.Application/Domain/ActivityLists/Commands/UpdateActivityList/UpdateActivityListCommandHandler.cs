using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityList
{
    public class UpdateActivityListCommandHandler : IRequestHandler<UpdateActivityListCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public UpdateActivityListCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateActivityListCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.ActivityLists.FindAsync(request.Id);
            entity.Name = request.Name;
            
            entity.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == entity.ProjectId, cancellationToken);
            entity.ActivityListStatus = request.ActivityListStatus;

            entity.SprintId = request.SprintId;
            entity.StartDate = request.StartDate;
            entity.DueDate = request.DueDate;
            entity.Description = request.Description;

            _context.ActivityLists.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            var notification = new ActivityListUpdated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                GroupEntityId = entity.ProjectId,
                GroupEntityName = entity.Project.Name,
                Recipients = _context.ProjectMembers.Include(x => x.User)
                    .Where(x => x.ProjectId == entity.ProjectId).Select(x => x.User.Email).ToList(),
                UserName = user.Identity.Name
            };

            await _mediator.Publish(notification, cancellationToken);
            
            return Unit.Value;
        }
    }
}
