using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent
{
    public class GetDepartmentTeamsByParentQuery: IRequest<DepartmentTeamsByParentModel>
    {
        public Guid DepartmentId { get; set; }
    }
}
