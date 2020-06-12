using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectInvoice
{
    public class GetProjectInvoiceQuery : IRequest<ProjectInvoiceModel>
    {
        public Guid ProjectId { get; set; }
    }
}
