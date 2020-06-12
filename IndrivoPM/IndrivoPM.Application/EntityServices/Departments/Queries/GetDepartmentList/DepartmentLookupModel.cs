using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList
{
    public class DepartmentLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeletable { get; set; }

        public bool Active { get; set; }

        public Guid? BusinessUnitId { get; set; }

        public Guid? DepartmentLeadId { get; set; }

        public string DepartmentLeadFullName { get; set; }

        public string DepartmentLeadJobPosition { get; set; }

        public string BusinessUnitName { get; set; }

        public List<DepartmentTeamsByParentLookup> DepartmentTeams { get; set; }


        public static Expression<Func<Department, DepartmentLookupModel>> Projection
        {
            get
            {
                return department => new DepartmentLookupModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    IsDeletable = department.IsDeletable,
                    Active = department.Active,
                    BusinessUnitId = department.BusinessUnitId,
                    BusinessUnitName = department.BusinessUnit != null && department.BusinessUnit.IsDeletable ? department.BusinessUnit.Name : "",
                    DepartmentLeadId = department.DepartmentLeadId,
                    DepartmentLeadJobPosition = department.DepartmentLead != null 
                                                && department.DepartmentLead.JobPosition != null 
                        ? department.DepartmentLead.JobPosition.Name : "",
                    DepartmentLeadFullName = department.DepartmentLead != null ?
                        department.DepartmentLead.FirstName + " " + department.DepartmentLead.LastName : "Not Specified",
                    DepartmentTeams = department.DepartmentTeams.Select(x => DepartmentTeamsByParentLookup.Create(x)).ToList()
                };
            }
        }

        public static DepartmentLookupModel Create(Department department)
        {
            return Projection.Compile().Invoke(department);
        }
    }
}
