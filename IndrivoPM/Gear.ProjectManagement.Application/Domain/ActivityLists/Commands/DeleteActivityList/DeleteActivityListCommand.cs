using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.DeleteActivityList
{
    public class DeleteActivityListCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
