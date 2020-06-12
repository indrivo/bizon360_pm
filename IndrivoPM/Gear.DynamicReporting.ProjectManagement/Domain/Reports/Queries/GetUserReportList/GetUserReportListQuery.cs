using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportList
{
    public class GetUserReportListQuery : IRequest<UserReportListViewModel>
    {
        public Guid UserId { get; set; }
    }
}
