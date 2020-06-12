using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.DeleteLoggedTime
{
   public class DeleteLoggedTimeCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
