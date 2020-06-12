using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Background.SendOverdueAttachments
{
    public class TeamMembersDto
    {
        public IEnumerable<Guid> EmployeeIds { get; set; }

        public Guid ProjectId { get; set; }

        public static TeamMembersDto Create(Project project)
        {
            return new TeamMembersDto
            {
                EmployeeIds = project.ProjectMembers != null && project.ProjectMembers.Any()
                    ? project.ProjectMembers.Select(x => x.UserId)
                    : new List<Guid>(),
                ProjectId = project.Id
            };
        }
    }
}
