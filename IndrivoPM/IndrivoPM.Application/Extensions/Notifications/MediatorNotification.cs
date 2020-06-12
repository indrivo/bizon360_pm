using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.Extensions.Notifications
{
    public class MediatorNotification : INotification
    {
        public Guid PrimaryEntityId { get; set; } = Guid.NewGuid();

        public string PrimaryEntityName { get; set; } = "";

        public Guid GroupEntityId { get; set; } = Guid.NewGuid();

        public string GroupEntityName { get; set; } = "";

        public List<string> Recipients { get; set; } = new List<string>();

        /// <summary>
        /// User name who triggered the notification
        /// </summary>
        public string UserName { get; set; } = "";


        public async Task GetUsersFromProfilesAsync(IGearContext context, INotificationService notification)
        {
            var users = await RecipientsExtension.GetUsers(this.GetType().Name, context, notification);

            if (UserName != "") users.Remove(UserName);

            this.Recipients.AddRange(users);
        }

        public Message CreateNotificationMessage()
        {
            return new Message()
            {
                EventName = this.GetType().Name,
                PrimaryEntityGroup = this.PrimaryEntityId,

                PrimaryEntityGroupName = this.GroupEntityName,
                SentTime = DateTime.Now,

                To = this.Recipients.Any() ? this.
                    Recipients.Distinct().Aggregate((a, b) => a + "; " + b) : "",
                Subject = this.GetType().Name.SplitCamelCase()
            };
        }
    }
}
