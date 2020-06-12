using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList
{
    public class GetDepartmentTeamListQuery : IRequest<DepartmentTeamListViewModel>
    {
        public Guid DepartmentId { get; set; } = new Guid();

    }
}
