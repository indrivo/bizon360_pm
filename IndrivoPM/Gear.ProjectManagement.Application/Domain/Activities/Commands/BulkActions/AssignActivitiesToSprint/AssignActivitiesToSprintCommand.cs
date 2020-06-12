using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.AssignActivitiesToSprint
{
    public class AssignActivitiesToSprintCommand : IRequest
    {
        public ICollection<Guid> ActivitiesById { get; set; }

        [DisplayName("Sprint")]
        public Guid SprintId { get; set; }
    }
}
