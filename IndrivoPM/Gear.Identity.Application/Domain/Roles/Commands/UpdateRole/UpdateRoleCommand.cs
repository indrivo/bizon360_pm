using System.Collections.Generic;
using System.ComponentModel;
using Gear.Identity.Manager.Domain.Roles.Queries.GetPermissionList;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest
    {
        [DisplayName("Name")]
        public string RoleName { get; set; }

        public string Description { get; set; }

        public List<Permissions.Domain.Resources.Permissions> Permissions { get; set; }

        public PermissionListViewModel PermissionsList { get; set; }
    }
}
