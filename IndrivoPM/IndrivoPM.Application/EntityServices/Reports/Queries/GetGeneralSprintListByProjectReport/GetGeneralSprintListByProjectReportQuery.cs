using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport
{
    public class GetGeneralSprintListByProjectReportQuery : IRequest<SprintListGeneralReportListViewModel>
    {
        public Guid Id { get; set; }
    }
}
