using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUsers
{
    public class DeleteApplicationUsersCommand : IRequest
    {
        public ICollection<Guid> UsersById { get; set; }
    }
}
