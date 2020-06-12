using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportProjectStatuses
{
    public class UpdateUserReportProjectStatusesCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public IList<ProjectStatus> ProjectStatuses { get; set; }
    }
}
