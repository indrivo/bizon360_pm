using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;

namespace Gear.Domain.HrmEntities
{
    public class BusinessUnit : BaseModel
    {
        public string Description { get; set; }

        public Guid? BusinessUnitLeadId { get; set; }

        public string Address { get; set; }

        // -------------------------------------------
        // Relationship

        public ApplicationUser BusinessUnitLead { get; set; }

        public ICollection<Department> Department{ get; set; }
    }
}
