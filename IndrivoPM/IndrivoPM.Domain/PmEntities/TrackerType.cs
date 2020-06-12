using System;
using System.Collections.Generic;
using Gear.Common.Entities;

namespace Gear.Domain.PmEntities
{
   public class TrackerType:BaseModel
    {
        public Guid? ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; }
        
        public ICollection<LoggedTime> LoggedTimes { get; set; }

        public int? RowOrder { get; set; }
    }
}
