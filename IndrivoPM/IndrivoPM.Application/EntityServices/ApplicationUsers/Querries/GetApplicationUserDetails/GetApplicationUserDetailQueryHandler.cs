using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserDetails
{
    public class GetApplicationUserDetailQueryHandler : IRequestHandler<GetApplicationUserDetailQuery, ApplicationUserDetailModel>
    {
        private readonly IGearContext _context;

        public GetApplicationUserDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUserDetailModel> Handle(GetApplicationUserDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ApplicationUsers
                .Include(p => p.Department)
                .Include(x => x.DepartmentTeam)
                .Include(x => x.UserDepartmentTeams).ThenInclude(x => x.DepartmentTeam)
                .Include(x => x.Projects)
                .FirstAsync(u => u.Id == request.Id, cancellationToken);
            
            if (entity == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.Id);
            }

            return ApplicationUserDetailModel.Create(entity);
        }
    }
}
