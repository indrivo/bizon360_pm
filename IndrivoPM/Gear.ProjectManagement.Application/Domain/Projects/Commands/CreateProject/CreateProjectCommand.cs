using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest
    {
        [DisplayName("Project group")]
        public Guid? ProjectGroupId { get; set; }

        public string Name { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        [DisplayName("Project URL")]
        public string ProjectUrl { get; set; }
        
        public ProjectStatus Status { get; set; }

        public ActivityPriority Priority { get; set; }

        [DisplayName("Start date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Due date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Project manager")]
        public Guid? ProjectManagerId { get; set; }

        [DisplayName("Team")]
        public ICollection<Guid> ProjectMemberIds { get; set; }

        public string Description { get; set; }
    }
}