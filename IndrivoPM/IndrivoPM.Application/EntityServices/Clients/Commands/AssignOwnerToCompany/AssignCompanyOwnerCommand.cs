using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AssignOwnerToCompany
{
    public class AssignCompanyOwnerCommand : IRequest
    {
        public Guid CompanyId { get; set; }

        public Guid OwnerId { get; set; }
    }
}
