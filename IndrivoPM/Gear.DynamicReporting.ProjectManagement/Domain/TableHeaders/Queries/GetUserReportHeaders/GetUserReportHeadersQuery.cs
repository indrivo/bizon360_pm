using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetUserReportHeaders
{
    public class GetUserReportHeadersQuery : IRequest<UserReportHeaderListViewModels>
    {
        public Guid UserId { get; set; }
        
        public Guid ReportId { get; set; }
    }
}
