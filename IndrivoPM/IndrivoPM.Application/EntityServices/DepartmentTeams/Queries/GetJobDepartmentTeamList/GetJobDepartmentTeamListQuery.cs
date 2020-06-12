using System;
using System.Collections.Generic;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetJobDepartmentTeamList
{
    public class GetJobDepartmentTeamListQuery:IRequest<JobPositionListViewModel>
    {
        public List<Guid> TeamIds { get; set; }
    }
}
