using System;
using System.Collections.Generic;
using Gear.Domain.HrmEntities;
using Gear.Domain.HrmEntities.Enums;
using Gear.Domain.HrmInterfaces;
using Gear.Domain.PmEntities;
using Gear.Domain.PmInterfaces;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Gear.Domain.AppEntities
{
    public class ApplicationUser : IdentityUser<Guid>, IApplicationUser, IPmApplicationUser, IHrmApplicationUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EmploymentType EmploymentType { get; set; } = EmploymentType.PartTime4H;

        public ICollection<ProjectMember> Projects { get; set; }

        public ICollection<ActivityAssignee> Activities { get; set; }

        public ICollection<UserDepartmentTeam> UserDepartmentTeams { get; set; }

        public ICollection<Department>  Department { get; set; }

        public ICollection<BusinessUnit>  BusinessUnits { get; set; }

        public ICollection<DepartmentTeam>  DepartmentTeam { get; set; }

        public ICollection<LoggedTime> LoggedTime { get; set; }

        public Guid? JobPositionId { get; set; }

        public JobPosition JobPosition { get; set; }

        public bool Active { get; set; } = true;

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }
    }
}