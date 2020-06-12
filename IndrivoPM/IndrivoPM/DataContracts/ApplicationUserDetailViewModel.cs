using System;
using Gear.Domain.HrmEntities.Enums;

namespace Bizon360.DataContracts
{
    public class ApplicationUserDetailViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        public string PhoneNumber { get; set; }

        public string JobPositionName { get; set; }

        public Guid? JobPositionId { get; set; }

        public bool Active { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public string BusinessUnits { get; set; }

        public string Departments { get; set; }

        public string Teams { get; set; }

        public string RoleNames { get; set; }

        public DateTime Created { get; set; }
    }
}
