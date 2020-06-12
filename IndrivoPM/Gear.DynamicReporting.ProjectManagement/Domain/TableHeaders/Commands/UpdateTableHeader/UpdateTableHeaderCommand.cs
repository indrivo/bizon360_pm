using System;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.UpdateTableHeader
{
    public class UpdateTableHeaderCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public bool Active { get; set; }

        public bool Deletable { get; set; }
    }
}
