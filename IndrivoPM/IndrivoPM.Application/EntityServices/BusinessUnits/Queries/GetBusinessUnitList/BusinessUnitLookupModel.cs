using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;
using Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitList
{
    public class BusinessUnitLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeletable { get; set; }

        public bool Active { get; set; }

        public string Address { get; set; }

        public Guid? BusinessUnitLeadId { get; set; }

        public string BusinessUnitLeadFullName { get; set; }

        public string BusinessUnitLeadJobPosition { get; set; }

        public List<DepartmentLookupByParentModel> DepartmentLookups { get; set; }

        public Dictionary<Guid, string> Departments { get; set; }


        public static Expression<Func<BusinessUnit, BusinessUnitLookupModel>> Projection
        {
            get
            {
                return businessUnit => new BusinessUnitLookupModel
                {
                    Id = businessUnit.Id,
                    Name = businessUnit.Name,
                    Address = businessUnit.Address,
                    Description = businessUnit.Description,
                    IsDeletable = businessUnit.IsDeletable,
                    Active = businessUnit.Active,
                    BusinessUnitLeadId = businessUnit.BusinessUnitLeadId,
                    BusinessUnitLeadFullName = businessUnit.BusinessUnitLead != null
                        ? businessUnit.BusinessUnitLead.FirstName + " " + businessUnit.BusinessUnitLead.LastName
                        : "Not Specified",
                    BusinessUnitLeadJobPosition =
                        businessUnit.BusinessUnitLead != null && businessUnit.BusinessUnitLead.JobPosition != null
                            ? businessUnit.BusinessUnitLead.JobPosition.Name
                            : "",
                    DepartmentLookups = businessUnit.Department.Select(x => DepartmentLookupByParentModel.Create(x)).ToList(),
                    Departments = businessUnit.Department != null
                        ? businessUnit.Department.ToDictionary(d => d.Id, d => d.Name)
                        : new Dictionary<Guid, string>()

                };
            }
        }

        public static BusinessUnitLookupModel Create(BusinessUnit businessUnit)
        {
            return Projection.Compile().Invoke(businessUnit);
        }
    }
}
