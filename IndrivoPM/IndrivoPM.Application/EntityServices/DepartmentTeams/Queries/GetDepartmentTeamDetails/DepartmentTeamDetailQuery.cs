using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamDetails
{
    public class DepartmentTeamDetailQuery:IRequest<DepartmentTeamDetailModel>
    {
        public Guid Id { get; set; }
    }
}
