using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeader
{
    public class ActivateTableHeaderCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
