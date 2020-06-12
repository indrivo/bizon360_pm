using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails
{
    public class GetUserReportDetailsQuery : IRequest<UserReportDetailModel>
    {
        public Guid UserId { get; set; }
        
        public Guid ReportId { get; set; }
    }
}
