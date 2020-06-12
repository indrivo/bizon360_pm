using System;
using System.Collections.Generic;
using System.Text;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport.DTOs
{
    public class ProjectGroupLogsDto : IEquatable<ProjectGroupLogsDto>
    {
        public Guid ProjectGroupId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid SprintId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }

        public Guid LoggedTimeId { get; set; }


        public string ProjectGroupName { get; set; }

        public string ProjectName { get; set; }

        public string SprintName { get; set; }

        public string ActivityName { get; set; }

        public string AssigneeName { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Equals(ProjectGroupLogsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ProjectGroupId.Equals(other.ProjectGroupId) && ProjectId.Equals(other.ProjectId) &&
                   SprintId.Equals(other.SprintId) && ActivityId.Equals(other.ActivityId) &&
                   AssigneeId.Equals(other.AssigneeId) && LoggedTimeId.Equals(other.LoggedTimeId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectGroupLogsDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ProjectGroupId.GetHashCode();
                hashCode = (hashCode * 397) ^ ProjectId.GetHashCode();
                hashCode = (hashCode * 397) ^ SprintId.GetHashCode();
                hashCode = (hashCode * 397) ^ ActivityId.GetHashCode();
                hashCode = (hashCode * 397) ^ AssigneeId.GetHashCode();
                hashCode = (hashCode * 397) ^ LoggedTimeId.GetHashCode();
                return hashCode;
            }
        }
    }
}
