using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportGuids
{
    public class AddUserReportGuidsCommand<TUserIdType> : IRequest
    {
        public Guid ReportId { get; set; }

        public TUserIdType UserId { get; set; }

        public IList<Guid> GuidList { get; set; }
    }
}
