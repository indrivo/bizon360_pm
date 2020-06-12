using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById
{
    public class GetTableHeaderDetailsByIdQuery : IRequest<TableHeaderDetailModel>
    {
        public Guid TableHeaderId { get; set; }
    }
}
