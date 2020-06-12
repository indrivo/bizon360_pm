using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.UpdateHeadline
{
    public class UpdateHeadlineCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
