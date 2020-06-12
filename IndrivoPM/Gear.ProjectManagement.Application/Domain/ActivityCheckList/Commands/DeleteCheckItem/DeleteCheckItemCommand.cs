using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.DeleteCheckItem
{
    public class DeleteCheckItemCommand : IRequest
    {
        public Guid CheckItemId { get; set; }
    }
}
