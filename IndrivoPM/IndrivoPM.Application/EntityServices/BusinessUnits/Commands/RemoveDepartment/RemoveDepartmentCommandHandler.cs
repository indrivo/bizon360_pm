using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Sstp.Abstractions.Exceptions;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RemoveDepartment
{
    public class RemoveDepartmentCommandHandler : IRequestHandler<RemoveDepartmentCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        public RemoveDepartmentCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(RemoveDepartmentCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            var notDeletableBusinessUnit = _context.BusinessUnits.First(x => x.IsDeletable == false).Id;

            foreach (var item in request.DepartmentIds)
            {
                var entity = await _context.Departments.FindAsync(item);
                if(entity == null) throw new NotFoundException("Cannot find department by id", nameof(Department));
                entity.BusinessUnitId = notDeletableBusinessUnit;

                _context.Departments.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
