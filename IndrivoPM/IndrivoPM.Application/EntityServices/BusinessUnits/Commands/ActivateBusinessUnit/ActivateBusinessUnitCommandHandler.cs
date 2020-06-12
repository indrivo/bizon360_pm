using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.ActivateBusinessUnit
{
    public class ActivateBusinessUnitCommandHandler : IRequestHandler<ActivateBusinessUnitCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public ActivateBusinessUnitCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(ActivateBusinessUnitCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.BusinessUnits.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null) return Unit.Value;

            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);


            if (request.Active)
            {
                entity.Activate();
            }
            else
            {
                entity.Deactivate();

                var hasDepartments = _context.Departments.Where(x => x.BusinessUnitId == request.Id);

                if (hasDepartments.Any())
                    foreach (var item in hasDepartments)
                    {
                        item.Deactivate();
                        _context.Departments.Update(item);
                    }
            }

            _context.BusinessUnits.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
