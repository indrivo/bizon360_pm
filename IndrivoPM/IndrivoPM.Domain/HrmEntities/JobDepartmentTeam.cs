using System;

namespace Gear.Domain.HrmEntities
{
    public class JobDepartmentTeam
    {
        public Guid DepartmentTeamId { get; set; }

        public DepartmentTeam DepartmentTeam { get; set; }

        public Guid JobPositionId { get; set; }

        public JobPosition JobPosition { get; set; }
    }
}
