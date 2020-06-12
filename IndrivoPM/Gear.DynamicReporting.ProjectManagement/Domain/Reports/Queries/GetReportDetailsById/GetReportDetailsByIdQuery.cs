using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsById
{
    public class GetReportDetailsByIdQuery : IRequest<ReportDetailModel>
    {
        public Guid ReportId { get; set; }
    }
}
