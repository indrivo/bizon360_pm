using System.Collections.Generic;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Commands.ActivateRole
{
    public class ActivateRoleCommand : IRequest
    {
        public List<string> RoleNames { get; set; }

        public bool Active { get; set; }
    }
}
