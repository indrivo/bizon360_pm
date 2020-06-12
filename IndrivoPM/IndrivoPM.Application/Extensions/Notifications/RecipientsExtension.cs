using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;

namespace Gear.Manager.Core.Extensions.Notifications
{
    public static class RecipientsExtension
    {
        public static async Task<List<string>> GetUsers(
            string eventName, IGearContext context, INotificationService notificationService)
        {
            var currentRecipients = new List<string>();
            if (!notificationService.CheckIfEventProfileExists(eventName)) return currentRecipients;
            var users = await notificationService.GetUsers
                (eventName, new List<IApplicationUser>(context.ApplicationUsers.ToList()));
            if (users.Any())
                currentRecipients.AddRange(users);

            return currentRecipients;
        }
    }
}
