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

namespace Gear.Manager.Core.EntityServices.Departments.Commands.ActivateDepartment
{
    public class ActivateDepartmentCommandHandler : IRequestHandler<ActivateDepartmentCommand>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;

        public ActivateDepartmentCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(ActivateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            if(request.Ids.Count == 0) throw new IdNullOrEmptyException();

            foreach (var item in request.Ids)
            {
                var entity = _context.Departments.Find(item);

                

                if (request.Active)
                {
                    entity.Activate();
                }
                else
                {
                    entity.Deactivate();

                    // Check if this department contains teams.
                    var hasTeams = _context.DepartmentTeams.Where(x => x.DepartmentId == item);

                    // If this department contains team sets all to inactive.
                    if (hasTeams.Any())
                        foreach (var team in hasTeams)
                        {
                            team.Deactivate();
                            _context.DepartmentTeams.Update(team);
                        }
                }
                _context.Departments.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
