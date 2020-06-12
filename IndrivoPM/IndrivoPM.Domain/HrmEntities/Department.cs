using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;

namespace Gear.Domain.HrmEntities
{
    public class Department : BaseModel
    {
        public string Description { get; set; }

        public Guid? BusinessUnitId { get; set; }

        public Guid? DepartmentLeadId { get; set; }

        public string Abbreviation { get; set; }

        public int RowOrder { get; set; }

        // -------------------------------------------
        // Relationship

        public BusinessUnit BusinessUnit { get; set; }

        public ApplicationUser DepartmentLead { get; set; }

        public ICollection<DepartmentTeam> DepartmentTeams { get; set; }
    }
}