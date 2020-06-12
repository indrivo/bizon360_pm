using System;
using Gear.Domain.AppEntities;

namespace Gear.Domain.PmEntities
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}