using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.RefuseChangeRequest
{
    public class RefuseChangeRequestCommandHandler : IRequestHandler<RefuseChangeRequestCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public RefuseChangeRequestCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RefuseChangeRequestCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.ChangeRequests
                .Include(cr => cr.Project)
                .FirstOrDefaultAsync(cr => cr.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ChangeRequest), request.Id);
            }

            entity.Comment = request.Comment;
            entity.Status = ChangeRequestStatus.Refused;
            entity.ReviewBy =  Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            entity.ReviewDate = DateTime.Now;

            _context.ChangeRequests.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);




            // Notifications
            await _mediator.Publish(new ChangeRequestRefused
            {
                PrimaryEntityName = entity.Name,
                PrimaryEntityId = entity.Id,
                GroupEntityName = entity.Project.Name,
                Recipients = new List<string> { _context.ApplicationUsers.Find(entity.CreatedBy).Email },
                GroupEntityId = entity.ProjectId,
                UserName = user.Identity.Name
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
