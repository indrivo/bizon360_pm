using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;

namespace Gear.Domain.HrmEntities
{
    public class DepartmentTeam : BaseModel
    {
        public string Description { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? DepartmentTeamLeadId { get; set; }

        public string Abbreviation { get; set; }

        public int RowOrder { get; set; }


        // ----------------------------------------
        // Relationship
        public Department Department { get; set; }

        public ApplicationUser DepartmentTeamLead { get; set; }

        public ICollection<UserDepartmentTeam> UserDepartmentTeams { get; set; }

        public ICollection<JobDepartmentTeam> JobDepartmentTeams { get; set; }
    }

}