using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.MoveDepartment
{
    public class MoveDepartmentCommandHandler : IRequestHandler<MoveDepartmentCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public MoveDepartmentCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(MoveDepartmentCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            foreach (var item in request.DepartmentIds)
            {
                var department = await _context.Departments.FindAsync(item);

                if (department == null) throw new IdNullOrEmptyException();

                var notDeletableUnitId =_context.BusinessUnits.First(x => !x.IsDeletable).Id;

                if (request.BusinessUnitId == null || request.BusinessUnitId == Guid.Empty)
                {
                    department.BusinessUnitId = notDeletableUnitId;

                    _context.Departments.Update(department);

                    await _context.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                department.BusinessUnitId = request.BusinessUnitId;

                _context.Departments.Update(department);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
