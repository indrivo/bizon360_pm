using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.AddToRoles
{
    public class AddToRolesCommand : IRequest
    {
        [DisplayName("Role")]
        public IList<string> RoleNames { get; set; }

        public Guid UserId { get; set; }
    }
}
