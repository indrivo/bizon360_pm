using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.RenameCheckItem
{
    public class RenameCheckItemCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
    }
}
