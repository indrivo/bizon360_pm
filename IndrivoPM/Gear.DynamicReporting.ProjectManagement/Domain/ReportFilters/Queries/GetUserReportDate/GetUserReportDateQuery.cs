using System;
using Gear.Domain.ReportEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportDate
{
    /// <summary>
    /// Gets Date (Start or Due) for a report of the user according to the
    /// input filter type parameter.
    /// </summary>
    public class GetUserReportDateQuery : IRequest<UserReportDate>
    {
        public Guid UserId { get; set; } 
        
        public Guid ReportId { get; set; } 
        
        public FilterType FilterType { get; set; }
    }
}
