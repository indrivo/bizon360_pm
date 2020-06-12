using System.Collections.Generic;
using Gear.Domain.PmEntities;

namespace Gear.Domain.PmInterfaces
{
    public interface IPmApplicationUser
    {
        ICollection<ProjectMember> Projects { get; set; }

        ICollection<ActivityAssignee> Activities { get; set; }

        ICollection<LoggedTime> LoggedTime { get; set; }
    }
}
