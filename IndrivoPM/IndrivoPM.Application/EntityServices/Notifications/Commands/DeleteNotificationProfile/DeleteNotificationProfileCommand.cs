using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.DeleteNotificationProfile
{
    public class DeleteNotificationProfileCommand : IRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
