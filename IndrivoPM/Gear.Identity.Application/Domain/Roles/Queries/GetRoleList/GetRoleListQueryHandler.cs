using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Infrastructure.Resources.ViewModels;
using Gear.Identity.Permissions.Services;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetRoleList
{
    public class GetRoleListQueryHandler : IRequestHandler<GetRoleListQuery, List<RolesListDto>>
    {
        private readonly IPermissionContext _context;

        public GetRoleListQueryHandler(IPermissionContext context)
        {
            _context = context;
        }

        public Task<List<RolesListDto>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<RolesListDto>(new ListRoles(_context).ListRolesWithPermissionsExplained()));
        }
    }
}
