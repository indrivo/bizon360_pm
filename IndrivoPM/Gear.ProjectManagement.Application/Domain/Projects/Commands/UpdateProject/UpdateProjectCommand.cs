using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Project group")]
        public Guid? ProjectGroupId { get; set; }

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

        [DisplayName("Notes")]
        public string Description { get; set; }

        [DisplayName("Project link")]
        public string ProjectUrl { get; set; }

        [DisplayName("Estimated budget")]
        public decimal? Budget { get; set; }

        [DisplayName("Solution type")]
        public Guid? SolutionTypeId { get; set; }

        [DisplayName("Service type")]
        public Guid? ServiceTypeId { get; set; }

        [DisplayName("Product type")]
        public Guid? ProductTypeId { get; set; }

        [DisplayName("Technology type")]
        public Guid? TechnologyTypeId { get; set; }
    }
}