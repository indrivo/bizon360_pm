using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.DeactivateUserReportHeader
{
    public class DeactivateUserReportHeaderCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public Guid TableHeaderId { get; set; }
    }
}
