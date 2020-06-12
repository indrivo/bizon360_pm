using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent
{
    public class GetDepartmentByParentQuery : IRequest<DepartmentByParentModel>
    {
        public Guid BusinessUnitId { get; set; }
    }
}
