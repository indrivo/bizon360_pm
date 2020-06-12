using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.AddTableHeaderToUserReport
{
    public class AddTableHeaderToUserReportCommand<TUserIdType> : IRequest<Unit>
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public Guid TableHeaderId { get; set; }
    }
}
