using System;
using Gear.Domain.ReportEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportDate
{
    public class AddUserReportDateCommand<TUserIdType> : IRequest
    {
        public TUserIdType UserId { get; set; }

        public Guid ReportId { get; set; }

        public FilterType FilterType { get; set; }

        public DateTime Date { get; set; }
    }
}
