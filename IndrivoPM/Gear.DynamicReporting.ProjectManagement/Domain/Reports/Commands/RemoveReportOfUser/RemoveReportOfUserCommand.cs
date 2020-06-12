using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.RemoveReportOfUser
{
    public class RemoveReportOfUserCommand<TUserIdType> : IRequest
    {
        public TUserIdType UserId { get; set; }

        public Guid ReportId { get; set; }
    }
}
