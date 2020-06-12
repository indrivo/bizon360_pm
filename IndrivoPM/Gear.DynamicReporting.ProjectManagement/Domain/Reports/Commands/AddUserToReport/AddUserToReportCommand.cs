using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.AddUserToReport
{
    public class AddUserToReportCommand<TUserIdType> : IRequest
    {
        public TUserIdType UserId { get; set; }

        public Guid ReportId { get; set; }
    }
}
