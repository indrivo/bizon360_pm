using System;
using System.Collections.Generic;
using Gear.Domain.AppEntities;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfile
{
    public class NotificationProfileDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ApplicationUser> UserList { get; set; }

        public List<EventDto> EventList { get; set; }

    }
}
