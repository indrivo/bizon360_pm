using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.DeactivateTableHeader
{
    public class DeactivateTableHeaderCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
