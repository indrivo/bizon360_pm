using System;
using Gear.Domain.PmEntities.Enums;

namespace Bizon360.Models
{
    public class ActivityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? SprintEndDate { get; set; }

        public ActivityPriority Priority { get; set; }

        public string ProjectName { get; set; }
    }
}
