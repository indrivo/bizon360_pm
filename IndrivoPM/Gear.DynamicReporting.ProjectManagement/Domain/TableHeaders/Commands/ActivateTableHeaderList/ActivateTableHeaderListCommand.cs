using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeaderList
{
    public class ActivateTableHeaderListCommand : IRequest
    {
        public Guid UserId { get; set; }

        public Guid ReportId { get; set; }

        public IList<Guid> TableHeaderList { get; set; }
    }
}
