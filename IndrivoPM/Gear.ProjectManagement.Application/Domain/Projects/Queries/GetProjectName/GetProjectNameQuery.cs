using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectName
{
    public class GetProjectNameQuery : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
