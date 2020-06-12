using System;
using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportProjectStatuses
{
    public class AddUserReportProjectStatusesCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public IList<ProjectStatus> ProjectStatuses { get; set; }
    }
}
