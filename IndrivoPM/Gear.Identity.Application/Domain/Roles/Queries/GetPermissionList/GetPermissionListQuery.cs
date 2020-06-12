using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetPermissionList
{
    /// <summary>
    /// Get all permissions for a specific group
    /// </summary>
    public class GetPermissionListQuery : IRequest<PermissionListViewModel>
    {
    }
}
