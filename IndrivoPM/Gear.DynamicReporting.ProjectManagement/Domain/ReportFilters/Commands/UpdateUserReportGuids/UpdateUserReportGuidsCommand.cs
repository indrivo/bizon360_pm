using System;
using System.Collections.Generic;
using Gear.Domain.ReportEntities.Enums;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportGuids
{
    public class UpdateUserReportGuidsCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public FilterType FilterType { get; set; }

        public IList<Guid> GuidList { get; set; }
    }
}
