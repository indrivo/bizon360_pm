using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.PmEntities
{
    public class Activity : BaseModel
    {
        public string Description { get; set; }

        public ActivityPriority ActivityPriority { get; set; } = ActivityPriority.Low;

        public ActivityStatus ActivityStatus { get; set; } = ActivityStatus.New;

        public int Progress { get; set; } = 0;

        public float? EstimatedHours { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }
        
        public Guid? ActivityListId { get; set; }

        public ActivityList ActivityList { get; set; }

        public Guid? SprintId { get; set; }

        public Sprint Sprint { get; set; }

        public Guid ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; }

        public ICollection<LoggedTime> LoggedTimes { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public ICollection<ActivityAssignee> Assignees { get; set; }

        public int? RowOrder { get; set; }

        //public virtual ApplicationUser Author { get; set; }

        public int Number { get; set; }
        public Guid? ChnageRequestId { get; set; }
        public ChangeRequest ChnageRequest { get; set; }
    }
}