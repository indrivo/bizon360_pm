using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.CreateSection
{
    public class CreateSectionCommand : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string Content { get; set; }

        public Guid HeadlineId { get; set; }
    }
}
