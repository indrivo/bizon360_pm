using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.UpdateReport
{
    public class UpdateReportCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
