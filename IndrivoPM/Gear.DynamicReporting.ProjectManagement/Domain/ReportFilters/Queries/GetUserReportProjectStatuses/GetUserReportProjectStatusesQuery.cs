using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportProjectStatuses
{
    public class GetUserReportProjectStatusesQuery : IRequest<UserReportProjectStatuses>
    {
        public Guid UserId { get; set; }

        public Guid ReportId { get; set; }
    }
}
