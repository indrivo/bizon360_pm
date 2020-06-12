using System.Collections.Generic;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityAudit
{
    public class ActivityAuditListView
    {
        public IList<ActivityAuditModel> Audits { get; set; }
    }
}
