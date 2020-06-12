using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.ActivateUserReportHeader
{
    public class ActivateUserReportHeaderCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public Guid TableHeaderId { get; set; }
    }
}
