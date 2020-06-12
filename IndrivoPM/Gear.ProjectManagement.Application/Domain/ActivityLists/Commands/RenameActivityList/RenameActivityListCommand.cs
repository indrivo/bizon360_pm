using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.RenameActivityList
{
    public class RenameActivityListCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
