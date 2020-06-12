using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetUsers
{
    public class GetUsersQuery : IRequest<UsersDto>
    {
        public ICollection<Guid> UsersIds { get; set; }
    }
}
