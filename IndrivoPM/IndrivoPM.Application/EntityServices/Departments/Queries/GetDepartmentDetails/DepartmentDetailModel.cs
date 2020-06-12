using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;
using Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentDetails
{
    public class DepartmentDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? BusinessUnitId { get; set; }

        public string BusinessUnitName { get; set; }

        public Guid? DepartmentLeadId { get; set; }

        public string DepartmentLeadFullName { get; set; }

        public string DepartmentLeadJobPosition { get; set; }

        public string Abbreviation { get; set; }

        public bool Active { get; set; }

        public List<DepartmentTeamLookupModel> DepartmentTeams { get; set; }

        public static Expression<Func<Department, DepartmentDetailModel>> Projection
        {
            get
            {
                return department => new DepartmentDetailModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Active = department.Active,
                    Abbreviation = department.Abbreviation,
                    Description = department.Description,
                    BusinessUnitId = department.BusinessUnitId,
                    BusinessUnitName = department.BusinessUnit != null ? department.BusinessUnit.Name : "",
                    DepartmentLeadId = department.DepartmentLeadId,
                    DepartmentTeams = department.DepartmentTeams.Select(x => DepartmentTeamLookupModel.Create(x)).ToList(),
                    DepartmentLeadJobPosition = department.DepartmentLead != null && department.DepartmentLead.JobPosition != null ? department.DepartmentLead.JobPosition.Name : "None",
                    DepartmentLeadFullName = department.DepartmentLead != null ? 
                        department.DepartmentLead.FirstName + " " + department.DepartmentLead.LastName : "Not Specified"
                };
            }
        }
        public static DepartmentDetailModel Create(Department department)
        {
            return Projection.Compile().Invoke(department);
        }
    }
}
