using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.DeleteSection
{
    public class DeleteSectionCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
