using System;
using Gear.Domain.ReportEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList
{
    public class GetUserReportGuidListQuery : IRequest<UserReportGuidListModel>
    {
        public Guid UserId { get; set; }

        public Guid ReportId { get; set; }

        public FilterType FilterType { get; set; }
    }
}
