using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList
{
    public class GetJobPositionListQuery: IRequest<JobPositionListViewModel>
    {
        public Guid DepartmentTeamId { get; set; }
    }
}
