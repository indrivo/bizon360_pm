using System;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetEvent
{
    public class GetEventDataQuery : IRequest<EventDto>
    {
        public Guid Id { get; set; }
    }
}
