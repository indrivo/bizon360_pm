using System;
using Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsById;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName
{
    public class GetReportDetailsByNameQuery : IRequest<ReportDetailModel>
    {
        public string Name { get; set; }
    }
}
