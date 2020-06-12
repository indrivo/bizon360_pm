using System;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;

namespace Gear.Domain.PmEntities
{
    public class CheckItem : BaseModel
    {
        public string Content { get; set; } = "No content Provided";

        public bool IsCompleted { get; set; } = false;

        public Guid ActivityId { get; set; }

        public Activity Activity { get; set; }

        public Guid? LoggedTimeId { get; set; }

        public LoggedTime LoggedTime { get; set; }

        public Guid ApplicationUserId { get; set; }

        public int Order { get; set; } = 0;

        /// <summary>
        /// Application User who completed the task
        /// </summary>
        public ApplicationUser ApplicationUser { get; set; }
    }
}
