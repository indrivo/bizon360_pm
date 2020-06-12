using System;
using System.Collections.Generic;
using Gear.Domain.HrmEntities;
using Gear.Domain.HrmEntities.Enums;

namespace Gear.Domain.HrmInterfaces
{
    public interface IHrmApplicationUser
    {
        ICollection<UserDepartmentTeam> UserDepartmentTeams { get; set; }

        ICollection<Department> Department { get; set; }

        ICollection<DepartmentTeam> DepartmentTeam { get; set; }

        Guid? JobPositionId { get; set; }

        JobPosition JobPosition { get; set; }

        EmploymentType EmploymentType { get; set; }
    }
}
