using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent
{
    public class DepartmentLookupByParentModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; } = "-";

        public bool Active { get; set; }

        public Guid? DepartmentLeadId { get; set; }
        
        public string DepartmentLeadFullName { get; set; }
        
        public Dictionary<Guid, string> DepartmentTeams { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }

        public int RowOrder { get; set; }


        public static Expression<Func<Department, DepartmentLookupByParentModel>> Projection
        {
            get
            {
                return department => new DepartmentLookupByParentModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Active = department.Active,
                    Abbreviation = department.Abbreviation,
                    RowOrder = department.RowOrder,
                    DepartmentLeadId = department.DepartmentLeadId,
                    DepartmentLeadFullName = department.DepartmentLead != null ?
                        department.DepartmentLead.FirstName + " " + department.DepartmentLead.LastName : "Not Specified",
                    DepartmentTeams = department.DepartmentTeams != null ? 
                        department.DepartmentTeams.ToDictionary(t => t.Id, t => t.Name) : new Dictionary<Guid, string>(),
                    CreatedTime = department.CreatedTime,
                    ModifiedTime = department.ModifyTime
                };
            }
        }

        public static DepartmentLookupByParentModel Create(Department department)
        {
            return Projection.Compile().Invoke(department);
        }
    }
}
