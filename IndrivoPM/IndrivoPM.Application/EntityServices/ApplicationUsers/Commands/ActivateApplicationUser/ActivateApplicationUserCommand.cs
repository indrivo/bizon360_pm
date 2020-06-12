using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.ActivateApplicationUser
{
    public class ActivateApplicationUserCommand:IRequest
    {
        public List<Guid> Ids { get; set; }

        public bool Active { get; set; }
    }
}
