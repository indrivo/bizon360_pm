using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Domain.PmEntities
{
    public class ActivityList : BaseModel
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public ActivityListStatus ActivityListStatus { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public Guid? SprintId { get; set; }

        public Sprint Sprint { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int Number { get; set; }

        public ActivityList()
        {

        }

        public ActivityList(Guid id)
            : base(id)
        {

        }
    }
}
