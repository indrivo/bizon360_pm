using System;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails
{
    public class ActivityAuditModel
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public EntityDto Data { get; set; }

        public DateTime ModificationTime { get; set; }
    }
}
