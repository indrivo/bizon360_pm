using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentList
{
    public class GetDepartmentListQuery : IRequest<DepartmentListViewModel>
    {
        public Guid? BusinessUnitId { get; set; }
    }
}
