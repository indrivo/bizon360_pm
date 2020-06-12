using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler:IRequestHandler<UpdateDepartmentCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public UpdateDepartmentCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _context.Departments.Find(request.Id);

            

            entity.Name = request.Name;
            entity.DepartmentLeadId = request.DepartmentLeadId;
            entity.Description = request.Description;
            entity.BusinessUnitId = request.BusinessUnitId;
            entity.Abbreviation = request.Abbreviation;

            if (request.Active)
                entity.Activate();
            else
                entity.Deactivate();

            var hasTeams = _context.DepartmentTeams.Where(x => x.DepartmentId == request.Id);

            if (hasTeams.Any() && !request.Active)
                foreach (var item in hasTeams)
                {
                    item.Deactivate();
                    _context.DepartmentTeams.Update(item);
                }

            _context.Departments.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
            
            var notification = new DepartmentUpdated()
            {
                UserName = user.Identity.Name,
                PrimaryEntityName = entity.Name,
                PrimaryEntityId = entity.Id,
                GroupEntityName = entity.Name,
                GroupEntityId = entity.Id
            };

            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
