using System;
using System.Collections.Generic;
using System.Text;
using Gear.Domain.PmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport.DTOs
{
    public class AssigneeLogsFilteredDto : IEquatable<AssigneeLogsFilteredDto>
    {
        public Guid AssigneeId { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ProjectId { get; set; }

        public string AssigneeName { get; set; }

        public string ActivityName { get; set; }

        public string ProjectName { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public float LoggedTime { get; set; }

        public float EstimatedTime { get; set; }

        public Guid LoggedTimeId { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Equals(AssigneeLogsFilteredDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AssigneeId.Equals(other.AssigneeId) && ActivityId.Equals(other.ActivityId) &&
                   LoggedTimeId.Equals(other.LoggedTimeId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AssigneeLogsFilteredDto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = AssigneeId.GetHashCode();
                hashCode = (hashCode * 397) ^ ActivityId.GetHashCode();
                hashCode = (hashCode * 397) ^ LoggedTimeId.GetHashCode();
                return hashCode;
            }
        }
    }
}
