using System;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.CreateSprintCommand
{
    public class CreateSprintCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ProjectId { get; set; }

        public SprintStatus Status { get; set; } = SprintStatus.New;

        [DisplayName("Start date")]
        public DateTime StartDate { get; set; }

        [DisplayName("Due date")]
        public DateTime EndDate { get; set; }
    }
}
