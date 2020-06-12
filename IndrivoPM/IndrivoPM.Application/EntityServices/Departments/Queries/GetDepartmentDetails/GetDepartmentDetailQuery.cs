using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentDetails
{
    public class GetDepartmentDetailQuery:IRequest<DepartmentDetailModel>
    {
        public Guid Id { get; set; }
    }
}
