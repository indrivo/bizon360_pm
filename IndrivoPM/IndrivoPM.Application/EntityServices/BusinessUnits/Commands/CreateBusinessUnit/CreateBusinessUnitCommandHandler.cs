using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.CreateBusinessUnit
{
    public class CreateBusinessUnitCommandHandler : IRequestHandler<CreateBusinessUnitCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public CreateBusinessUnitCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateBusinessUnitCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            var businessUnit = new BusinessUnit
            {
                Description = request.Description,
                BusinessUnitLeadId = request.BusinessUnitLeadId,
                Address = request.Address
            };

            businessUnit.CreateEnd(Guid.NewGuid(), request.Name,userId);

            if (request.Active)
                businessUnit.Activate();
            else
                businessUnit.Deactivate();

            await _context.BusinessUnits.AddAsync(businessUnit,cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
