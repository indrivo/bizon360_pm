using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateJobPosition
{
    public class UpdateJobPositionCommandHandler : IRequestHandler<UpdateJobPositionCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public UpdateJobPositionCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateJobPositionCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _context.JobPositions.Find(request.Id);

            entity.Name = request.Name;
            entity.HourlySalary = request.HourlySalary;
            entity.Description = request.Description;
            entity.Abbreviation = request.Abbreviation;

            if (request.Active)
                entity.Activate();
            else
                entity.Deactivate();

            _context.JobPositions.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new JobPositionUpdated
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
