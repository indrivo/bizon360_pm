using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Extensions.EntityComparison;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.UpdateChangeRequest
{
    public class UpdateChangeRequestCommandHandler : IRequestHandler<UpdateChangeRequestCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public UpdateChangeRequestCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateChangeRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.ChangeRequests
                .Include(cr => cr.Project).ThenInclude(p => p.ProjectManager)
                .FirstAsync(cr => cr.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ChangeRequest), request.Id);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.ProjectId = request.ProjectId;
            entity.Priority = request.Priority;
            

            var changes = _context.ChangeRequests.Include(cr => cr.Project).AsNoTracking()
                .First(cr => cr.Id == request.Id)
                .EnumeratePropertyDifferences(entity, _context)
                .Aggregate("", (current, change) => current + "\n" + change);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            await _mediator.Publish(new ChangeRequestUpdated
            {
                Changes = !string.IsNullOrEmpty(changes) ? changes : "",
                PrimaryEntityName = entity.Name,
                PrimaryEntityId = entity.Id,
                GroupEntityName = entity.Project.Name,
                UserName = user.Identity.Name,
                Recipients = new List<string> { entity.Project.ProjectManager.Email },
                GroupEntityId = entity.ProjectId,
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
