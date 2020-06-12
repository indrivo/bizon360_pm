using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.CreateReport
{
    public class CreateReportCommand : IRequest
    {
        public string Name { get; set; }
    }
}
