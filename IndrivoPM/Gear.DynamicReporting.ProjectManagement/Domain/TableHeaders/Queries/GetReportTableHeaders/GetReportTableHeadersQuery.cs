using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetReportTableHeaders
{
    public class GetReportTableHeadersQuery : IRequest<ReportTableHeaderListViewModel>
    {
        public Guid ReportId { get; set; }
    }
}
