using System;
using Gear.Common.Entities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.PmEntities
{
    public class ChangeRequest : BaseModel
    {
        public string Description { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
        public Activity Activity { get; set; }
        public ActivityPriority Priority { get; set; }

        public ChangeRequestStatus Status { get; set; }

        public int Number { get; set; }
        public string Comment { get; set; }
        public Guid ReviewBy { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
