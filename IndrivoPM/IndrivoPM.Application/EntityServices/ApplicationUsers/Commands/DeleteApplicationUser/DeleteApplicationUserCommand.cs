using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
