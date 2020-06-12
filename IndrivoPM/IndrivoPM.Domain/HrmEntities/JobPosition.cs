using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;

namespace Gear.Domain.HrmEntities
{
    public class JobPosition:BaseModel
    {
        public int? HourlySalary { get; set; }

        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public int  RowOrder { get; set; }

        public ICollection<JobDepartmentTeam> JobDepartmentTeams { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
