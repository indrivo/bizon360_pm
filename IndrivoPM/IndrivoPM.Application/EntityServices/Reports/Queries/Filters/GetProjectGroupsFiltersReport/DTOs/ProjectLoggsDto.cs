using System;
using Gear.Domain.PmEntities;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport.DTOs
{
    public class ProjectLogsDto : IEquatable<ProjectLogsDto>
    {
        public Guid AssigneeId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid ProjectGroupId { get; set; }

        public Guid LoggedTimeId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectGroupName { get; set; }

        public string AssigneeName { get; set; }

        public DateTime Date { get; set; }

        public float LoggedTime { get; set; }

        public bool Equals(ProjectLogsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AssigneeId.Equals(other.AssigneeId) && ProjectId.Equals(other.ProjectId) &&
                   ProjectGroupId.Equals(other.ProjectGroupId) && LoggedTimeId.Equals(other.LoggedTimeId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectLogsDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = AssigneeId.GetHashCode();
                hashCode = (hashCode * 397) ^ ProjectId.GetHashCode();
                hashCode = (hashCode * 397) ^ ProjectGroupId.GetHashCode();
                hashCode = (hashCode * 397) ^ LoggedTimeId.GetHashCode();
                return hashCode;
            }
        }
    }
}
