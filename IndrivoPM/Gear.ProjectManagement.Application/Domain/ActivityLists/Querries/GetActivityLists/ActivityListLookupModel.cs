using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists
{
    public class ActivityListLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int CompletedActivitiesCount { get; set; }

        public int ActivitiesCount { get; set; }
        public int TotalActivitiesCount { get; set; }
        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public bool IsCompleted { get; set; }
    }
}
