using System;
using System.Collections.Generic;
using System.Text;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportActivityStatuses
{
    public class UpdateUserReportActivityStatusesCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public IList<ActivityStatus> ActivityStatuses { get; set; }
    }
}
