using System;
using System.Collections;
using System.Collections.Generic;
using Gear.Domain.AppEntities;

namespace Gear.Domain.PmEntities
{
    public class ActivityAssignee
    {
        public Guid ActivityId { get; set; }
        public Guid UserId { get; set; }
        
        public Activity Activity { get; set; }
        public ApplicationUser User { get; set; }
    }
}
