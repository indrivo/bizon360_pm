using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfileList
{
    public class NotificationProfileListLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Dictionary<Guid, string> UserList { get; set; }

    }
}
