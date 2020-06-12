using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Infrastructure.Resources.ViewModels;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetPermissionList
{
    public class GetPermissionListQueryHandler : IRequestHandler<GetPermissionListQuery, PermissionListViewModel>
    {
        public Task<PermissionListViewModel> Handle(GetPermissionListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new PermissionListViewModel
            {
                Permissions = PermissionDisplay.GetPermissionsToDisplay(typeof(Permissions.Domain.Resources.Permissions))
            });
        }
    }
}
