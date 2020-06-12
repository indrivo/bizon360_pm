using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent
{
    public class DepartmentTeamsByParentModel
    {
        public Guid DepartmentId { get; set; }

        public IList<DepartmentTeamsByParentLookup> DepartmentTeams { get; set; }
    }
}
