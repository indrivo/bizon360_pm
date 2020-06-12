using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateNotificationProfile
{
    public class UpdateNotificationProfileCommand : IRequest
    {
        public Guid Id { get; set; }

        public List<Guid> UserList { get; set; } = new List<Guid>();

        public List<Guid> EventList { get; set; } = new List<Guid>();

        public string Name { get; set; } = "No name provided";
    }
}
