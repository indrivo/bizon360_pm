using System;
using System.Collections.Generic;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList
{
   public class DepartmentListViewModel
    {
        public Guid BusinessUnitId { get; set; }

        public IList<DepartmentLookupModel> Departments { get; set; }
    }
}
