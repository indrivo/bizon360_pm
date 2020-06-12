using Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsByName
{
    public class GetTableHeaderDetailsByNameQuery : IRequest<TableHeaderDetailModel>
    {
        public string Name { get; set; }
    }
}
