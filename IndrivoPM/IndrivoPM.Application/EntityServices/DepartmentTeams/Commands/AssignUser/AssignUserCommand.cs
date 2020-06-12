using System;
using System.Collections.Generic;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignUser
{
    public class AssignUserCommand : IRequest
    {
        public Guid DepartmentTeamId { get; set; }

        public Guid? DepartmentId { get; set; }

        [DisplayName("Employees")]
        public List<Guid> ApplicationUserIds { get; set; }
    }
}
