using System;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport.DTOs
{
    public class ActivityDto
    {
        public string AssigneeName { get; set; }

        public Guid ActivityId { get; set; }

        public string ActivityName { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public float LoggedTime { get; set; }

        public float EstimatedTime { get; set; }

        public Guid LoggedTimeId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
