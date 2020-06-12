using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.CreateNotificationProfile
{
    public class CreateNotificationProfileCommand :IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; } = "No Name Provided";

        [DisplayName("Users")]
        public List<Guid> UserList { get; set; } = new List<Guid>();

        [DisplayName("Events")]
        public List<Guid> EventList { get; set; } = new List<Guid>();
    }
}
