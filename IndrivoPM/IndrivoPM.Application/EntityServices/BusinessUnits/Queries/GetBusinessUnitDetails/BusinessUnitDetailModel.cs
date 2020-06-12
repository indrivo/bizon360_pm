using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitDetails
{
    public class BusinessUnitDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public Guid? BusinessUnitLeadId { get; set; }

        public string BusinessUnitLeadFullName { get; set; }

        public string BusinessUnitLeadJobPosition { get; set; }

        public bool Active { get; set; }

        public Dictionary<Guid, string> Departments { get; set; }

        public static Expression<Func<BusinessUnit, BusinessUnitDetailModel>> Projection
        {
            get
            {
                return businessUnit => new BusinessUnitDetailModel
                {
                    Id = businessUnit.Id,
                    Name = businessUnit.Name,
                    Active = businessUnit.Active,
                    Address = businessUnit.Address,
                    Description = businessUnit.Description,
                    BusinessUnitLeadId = businessUnit.BusinessUnitLeadId,
                    BusinessUnitLeadFullName = businessUnit.BusinessUnitLead != null
                        ? businessUnit.BusinessUnitLead.FirstName + " " + businessUnit.BusinessUnitLead.LastName
                        : "Not Specified",
                    BusinessUnitLeadJobPosition =
                        businessUnit.BusinessUnitLead != null && businessUnit.BusinessUnitLead.JobPosition != null
                            ? businessUnit.BusinessUnitLead.JobPosition.Name
                            : "None",
                    Departments = businessUnit.Department.ToDictionary(x => x.Id, x => x.Name)
                };
            }
        }
        public static BusinessUnitDetailModel Create(BusinessUnit businessUnit)
        {
            return Projection.Compile().Invoke(businessUnit);
        }

    }
}
