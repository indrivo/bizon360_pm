using System.Collections.Generic;
using Gear.Identity.Manager.Domain.Roles.Queries.GetPermissionList;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PlatformName { get; set; }

        public IList<Permissions.Domain.Resources.Permissions> Permissions { get; set; }

        public PermissionListViewModel PermissionsList { get; set; } = new PermissionListViewModel();
    }
}
