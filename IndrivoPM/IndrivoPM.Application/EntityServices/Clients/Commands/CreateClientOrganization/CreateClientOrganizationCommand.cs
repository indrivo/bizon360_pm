using System;
using Gear.Domain.ValueObjects;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.CreateClientOrganization
{
    public class CreateClientOrganizationCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public ContactInfo ContactInfo { get; set; }

    }
}
