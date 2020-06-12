using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.CreateActivity
{
    public class CreateActivityCommand : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Description { get; set; }
        public ICollection<Guid> Assignees { get; set; }

        [DisplayName("Project")]
        public Guid ProjectId { get; set; }

        public ActivityStatus Status { get; set; }

        public ActivityPriority Priority { get; set; }

        public int Progress { get; set; }

        [DisplayName("Estimated time")]
        public float? EstimatedHours { get; set; }

        [DisplayName("Start date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Due date")]
        public DateTime? DueDate { get; set; }

        [DisplayName("Activity type")]
        public Guid ActivityTypeId { get; set; }

        [DisplayName("Activity list")]
        public Guid? ActivityListId { get; set; }

        [DisplayName("Sprint")]
        public Guid? SprintId { get; set; }

        public Guid? ChangeRequestId { get; set; }
    }
}
