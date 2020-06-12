using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.CreateJobPosition
{
    public class CreateJobPositionCommandHandler : IRequestHandler<CreateJobPositionCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public CreateJobPositionCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateJobPositionCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            var entity = new JobPosition
            {
                Description = request.Description,
                HourlySalary = request.HourlySalary,
                Abbreviation = request.Abbreviation
            };

            entity.CreateEnd(Guid.NewGuid(), request.Name, userId);

            if (request.Active)
                entity.Activate();
            else
                entity.Deactivate();

            if (request.DepartmentTeamId != null)
            {
                var jobTeam = new JobDepartmentTeam
                {
                    DepartmentTeamId = request.DepartmentTeamId.Value,
                    JobPositionId = entity.Id
                };

                _context.JobDepartmentTeams.Add(jobTeam);
            }

            _context.JobPositions.Add(entity);

            await _mediator.Publish(new JobPositionCreated()
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            },cancellationToken);


            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
