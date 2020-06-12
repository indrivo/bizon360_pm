using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.CreateChangeRequest
{
    public class CreateChangeRequestCommandHandler : IRequestHandler<CreateChangeRequestCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public CreateChangeRequestCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateChangeRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = new ChangeRequest
            {
                Description = request.Description,
                Comment = request.Comment,
                ProjectId = request.ProjectId,
                Priority = request.Priority,
                Status = ChangeRequestStatus.Unprocessed
            };

            entity.CreateEnd(request.EntityId, request.Name, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            _context.ChangeRequests.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var project = _context.Projects
                .Include(p => p.ProjectManager)
                .First(p => p.Id == request.ProjectId);

            await _mediator.Publish(new ChangeRequestCreated
            {
                PrimaryEntityName = entity.Name,
                PrimaryEntityId = entity.Id,
                UserName = user.Identity.Name,
                GroupEntityName = project.Name,
                Recipients = new List<string> { project.ProjectManager.Email },
                GroupEntityId = entity.ProjectId
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
