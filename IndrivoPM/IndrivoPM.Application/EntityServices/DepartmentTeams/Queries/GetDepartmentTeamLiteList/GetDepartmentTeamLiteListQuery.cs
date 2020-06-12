using System;
using MediatR;

namespace Gear.Application.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList
{
    public class GetDepartmentTeamListQuery : IRequest<DepartmentTeamListViewModel>
    {
        public Guid DepartmentId { get; set; } = new Guid();

    }
}
