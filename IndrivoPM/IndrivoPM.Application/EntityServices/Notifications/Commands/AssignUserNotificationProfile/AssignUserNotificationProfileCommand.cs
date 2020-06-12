using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.AssignUserNotificationProfile
{
    public class AssignUserNotificationProfileCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Users")]
        public IList<Guid> UserListIds { get; set; }

    }
}
