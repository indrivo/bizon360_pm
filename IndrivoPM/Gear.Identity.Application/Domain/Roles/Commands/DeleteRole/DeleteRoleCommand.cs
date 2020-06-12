using System.Collections.Generic;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest
    {
        public List<string> RoleNames { get; set; }
    }
}
