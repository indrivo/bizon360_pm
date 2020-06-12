using System;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport.DTOs
{
    public class ProjectLoggsDto : IEquatable<ProjectLoggsDto>
    {
        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public string ActivityName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public string ActivityType { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public Guid ProjectId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid AssigneeId { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Equals(ProjectLoggsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ProjectName == other.ProjectName 
                   && AssigneeName == other.AssigneeName 
                   && ActivityName == other.ActivityName 
                   && ActivityStatus == other.ActivityStatus 
                   && ActivityType == other.ActivityType 
                   && EstimatedTime.Equals(other.EstimatedTime) 
                   && LoggedTime.Equals(other.LoggedTime) 
                   && ProjectId.Equals(other.ProjectId) 
                   && ActivityId.Equals(other.ActivityId) 
                   && AssigneeId.Equals(other.AssigneeId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProjectLoggsDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ProjectName != null ? ProjectName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AssigneeName != null ? AssigneeName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ActivityName != null ? ActivityName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) ActivityStatus;
                hashCode = (hashCode * 397) ^ (ActivityType != null ? ActivityType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ EstimatedTime.GetHashCode();
                hashCode = (hashCode * 397) ^ LoggedTime.GetHashCode();
                hashCode = (hashCode * 397) ^ ProjectId.GetHashCode();
                hashCode = (hashCode * 397) ^ ActivityId.GetHashCode();
                hashCode = (hashCode * 397) ^ AssigneeId.GetHashCode();
                return hashCode;
            }
        }
    }
}
