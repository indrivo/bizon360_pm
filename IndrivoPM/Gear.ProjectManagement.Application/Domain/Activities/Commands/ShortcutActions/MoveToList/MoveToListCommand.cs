using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToList
{
    public class MoveToListCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Activity list")]
        public Guid ActivityListId { get; set; }

    }
}
