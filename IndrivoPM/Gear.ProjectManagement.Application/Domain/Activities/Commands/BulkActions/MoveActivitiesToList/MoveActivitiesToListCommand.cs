using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MoveActivitiesToList
{
    public class MoveActivitiesToListCommand : IRequest
    {
        public ICollection<Guid> ActivitiesById { get; set; }

        [DisplayName("Activity list")]
        public Guid ActivityListId { get; set; }
    }
}
