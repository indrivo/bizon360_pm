using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.DeleteDepartament
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        public DeleteDepartmentCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            if (request.Ids.Count == 0) throw new NoNullAllowedException("List of ids cannot be null");

            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            foreach (var item in request.Ids)
            {
                var entity = await _context.Departments.FindAsync(item);

                // Check if this entity exist.
                if (entity == null) throw new NotFoundException(nameof(Department), item);

                // Check if this department is allowed to delete.
                if (!entity.IsDeletable) throw new DeleteFailureException(nameof(Department), item, DeleteFailureMessages.EntityNotDeletable.ToString());

                // Check if this department has teams.
                var hasTeams = _context.DepartmentTeams.Where(x => x.DepartmentId == item);

                // If teams exist, will set to inactive and moved to not deletable department.
                if (hasTeams.Any())
                {
                    foreach (var team in hasTeams)
                    {
                        team.DepartmentId = _context.Departments.First(x => x.IsDeletable == false).Id;
                        team.Deactivate();
                        _context.DepartmentTeams.Update(team);
                    }
                }
                _context.Departments.Remove(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
