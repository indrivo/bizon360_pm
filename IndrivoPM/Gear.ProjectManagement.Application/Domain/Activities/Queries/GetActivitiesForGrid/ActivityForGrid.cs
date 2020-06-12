using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivitiesForGrid
{
    public class ActivityForGrid
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<AssigneeDto> Assignees { get; set; }

        public ActivityPriority Priority { get; set; }

        public ActivityStatus Status { get; set; }

        public int Progress { get; set; }

        public ActivityType ActivityType { get; set; }

        public float? EstimatedHours { get; set; }

        public float? LoggedTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public Guid? ActivityListId { get; set; }

        public string ActivityListName { get; set; }

        public Guid? SprintId { get; set; }
        public int? RowOrder { get; set; }
    }
}
