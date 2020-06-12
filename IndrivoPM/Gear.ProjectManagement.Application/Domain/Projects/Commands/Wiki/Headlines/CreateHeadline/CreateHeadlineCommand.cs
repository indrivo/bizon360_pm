using System;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.CreateHeadline
{
    public class CreateHeadlineCommand : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProjectId { get; set; }

        [DisplayName("Tab name")]
        public string Title { get; set; }

        [DisplayName("Section name")]
        public string SectionName { get; set; }

        [DisplayName("Section content")]
        public string SectionBody { get; set; }

    }
}
