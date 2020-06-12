using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.HrmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommand:IRequest
    {
        public Guid Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("User Name")]
        public string Email { get; set; }
        
        [DisplayName("Employment Type")]
        public EmploymentType EmploymentType { get; set; }

        [DisplayName("Team")]
        public Guid DepartmentTeamId { get; set; }

        [DisplayName("Job Position")]
        public Guid? JobPositionId { get; set; }

        [DisplayName("Role")]
        public List<string> RoleNames { get; set; }

        public List<Guid> Projects { get; set; }
    }
}
