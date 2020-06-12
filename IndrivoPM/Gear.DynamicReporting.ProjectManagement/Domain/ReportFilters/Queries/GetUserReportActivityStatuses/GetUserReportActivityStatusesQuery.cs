using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportActivityStatuses
{
    public class GetUserReportActivityStatusesQuery : IRequest<UserReportActivityStatusesModel>
    {
        public Guid UserId { get; set; }

        public Guid ReportId { get; set; }
    }
}
