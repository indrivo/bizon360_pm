using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.CreateTableHeader
{
    public class CreateTableHeaderCommand : IRequest
    {
        public string Name { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public bool Active { get; set; }

        public bool Deletable { get; set; }
    }
}
