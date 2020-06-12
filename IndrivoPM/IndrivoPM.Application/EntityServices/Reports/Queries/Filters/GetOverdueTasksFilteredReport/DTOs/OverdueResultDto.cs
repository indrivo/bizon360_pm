using System;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport.DTOs
{
    public class OverdueResultDto
    {
        public Guid ProjectGroupId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }

        public string ProjectGroupName { get; set; }

        public string ActivityName { get; set; }

        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime? Deadline { get; set; }
    }
}
