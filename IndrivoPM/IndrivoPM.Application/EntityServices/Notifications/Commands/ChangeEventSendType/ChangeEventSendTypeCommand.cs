using System;
using Gear.Notifications.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.ChangeEventSendType
{
    public class ChangeEventSendTypeCommand : IRequest
    {
        public Guid Id { get; set; }

        public PropagationType Type { get; set; }
    }
}
