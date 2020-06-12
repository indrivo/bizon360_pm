using System;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs
{
    public class ActivityDto
    {
        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public string ActivityName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public string ActivityType { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }


        public DateTime CreateTime { get; set; }

        public Guid AssigneeId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ProjectId { get; set; }
    }
}
