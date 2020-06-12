using System.Collections.Generic;
using Gear.Identity.Permissions.Infrastructure.Resources.ViewModels;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetPermissionList
{
    public class PermissionListViewModel
    {
        public IList<PermissionDisplay> Permissions { get; set; }
    }
}
