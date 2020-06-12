using System.Collections.Generic;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetRoleDetails
{
    public class RoleDetailModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Platform { get; set; }

        public bool Active { get; set; }

        public IEnumerable<Permissions.Domain.Resources.Permissions> Permissions { get; set; }

        public List<ApplicationUserLookupModel> UserList { get; set; }

    }
}
