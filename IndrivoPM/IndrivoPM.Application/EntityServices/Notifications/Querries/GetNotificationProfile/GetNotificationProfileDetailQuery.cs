using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfile
{
    public class GetNotificationProfileDetailQuery : IRequest<NotificationProfileDetailModel>
    {
        public Guid Id { get; set; }
    }
}
