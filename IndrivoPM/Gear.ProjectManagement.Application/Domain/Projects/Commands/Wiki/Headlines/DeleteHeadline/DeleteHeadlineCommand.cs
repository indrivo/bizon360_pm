using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.DeleteHeadline
{
    public class DeleteHeadlineCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
