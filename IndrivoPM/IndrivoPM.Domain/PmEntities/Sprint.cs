using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.PmEntities
{
    public class Sprint : BaseModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid? ProjectId { get; set; }

        public SprintStatus SprintStatus { get; set; }

        public Project Project { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<ActivityList> ActivityLists { get; set; }
    }
}
