using System;

namespace Gear.Domain.PmEntities
{
    public class ProjectActivityType
    {
        public Guid ProjectId { get; set; }

        public Guid ActivityTypeId { get; set; }

        public bool Active { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime ModifyTime { get; set; }

        #region Navigation Properties
        public Project Project { get; set; }

        public ActivityType ActivityType { get; set; }
        #endregion
    }
}
