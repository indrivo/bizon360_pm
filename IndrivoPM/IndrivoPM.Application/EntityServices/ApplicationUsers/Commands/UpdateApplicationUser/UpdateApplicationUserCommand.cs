using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.HrmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.UpdateApplicationUser
{
    public class UpdateApplicationUserCommand : IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        public string Email { get; set; }

        [DisplayName("Phone (optional)")]
        public string PhoneNumber { get; set; }

        [DisplayName("Employment type")]
        public EmploymentType EmploymentType { get; set; }

        [DisplayName("Business unit")]
        public Guid BusinessUnits { get; set; }

        public Guid Departments { get; set; }

        [DisplayName("Team")]
        public List<Guid> TeamIds { get; set; }

        [DisplayName("Job position")]
        public Guid? JobPositionId { get; set; }

        [DisplayName("Roles & permissions")]
        public List<string> RoleNames { get; set; }

        public bool Active { get; set; }
        public List<Guid> Projects { get; set; }
    }
}
