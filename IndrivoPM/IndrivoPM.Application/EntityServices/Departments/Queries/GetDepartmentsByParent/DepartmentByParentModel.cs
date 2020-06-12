using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent
{
   public class DepartmentByParentModel
    {
        public Guid BusinessUnitId { get; set; }

        public IList<DepartmentLookupByParentModel> Departments { get; set; }
    }
}
