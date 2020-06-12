using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.ChangeUserreportState
{
    public class ChangeUserReportStateCommand : IRequest
    {
        public Guid UserId { get; set; }

        public Guid ReportId { get; set; }

        public bool Active { get; set; }
    }
}
