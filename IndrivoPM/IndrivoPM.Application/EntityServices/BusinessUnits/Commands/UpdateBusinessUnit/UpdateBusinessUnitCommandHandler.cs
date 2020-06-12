using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.UpdateBusinessUnit
{
    public class UpdateBusinessUnitCommandHandler : IRequestHandler<UpdateBusinessUnitCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public UpdateBusinessUnitCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateBusinessUnitCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _context.BusinessUnits.Find(request.Id);

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.BusinessUnitLeadId = request.BusinessUnitLeadId;
            entity.Address = request.Address;


            if (request.Active)
                entity.Activate();
            else
                entity.Deactivate();


/*            var hasDepartments = _context.Departments.Where(x => x.BusinessUnitId == request.Id);

            if (hasDepartments.Any())
                if (!request.Active)
                    foreach (var item in hasDepartments)
                    {
                        item.Deactivate();
                        _context.Departments.Update(item);
                    }*/

            _context.BusinessUnits.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
