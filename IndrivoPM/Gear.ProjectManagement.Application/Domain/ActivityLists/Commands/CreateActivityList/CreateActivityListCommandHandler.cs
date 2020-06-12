using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.CreateActivityList
{
    public class CreateActivityListCommandHandler : IRequestHandler<CreateActivityListCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CreateActivityListCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateActivityListCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = new ActivityList(request.Id)
            {
                Name = request.Name,
                ProjectId = request.ProjectId,
                ActivityListStatus = request.ActivityListStatus,
                SprintId = request.SprintId,
                Description = request.Description,
                StartDate = request.StartDate,
                DueDate = request.DueDate,
                Project = await _context.Projects.FindAsync(request.ProjectId),
                Sprint = await _context.Sprints.FindAsync(request.SprintId)
            };

            await _context.ActivityLists.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            var notification = new ActivityListCreated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                GroupEntityName = entity.Project.Name,
                GroupEntityId = entity.ProjectId,
                Recipients = _context.ProjectMembers.Include(x => x.User)
                    .Where(x => x.ProjectId == entity.ProjectId).Select(x => x.User.Email).ToList(),
                UserName = user.Identity.Name
            };

            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
