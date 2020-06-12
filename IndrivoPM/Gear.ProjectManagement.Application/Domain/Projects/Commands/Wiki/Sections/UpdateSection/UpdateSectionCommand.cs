using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.UpdateSection
{
    public class UpdateSectionCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
