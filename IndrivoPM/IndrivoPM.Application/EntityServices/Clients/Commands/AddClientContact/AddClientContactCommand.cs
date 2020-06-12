using System;
using Gear.Domain.ValueObjects;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AddClientContact
{
    public class AddClientContactCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public CompoundName Name { get; set; }

        public ContactInfo ContactInfo { get; set; }

        public string Role { get; set; } = "";
    }
}
