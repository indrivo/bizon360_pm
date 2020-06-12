using System;
using Gear.Domain.AppEntities;

namespace Gear.Domain.HrmEntities
{
    public class UserDepartmentTeam
    {
        public Guid DeparmentTeamId { get; set; }

        public DepartmentTeam DepartmentTeam { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
