using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitDetails
{
    public class GetBusinessUnitDetailQuery : IRequest<BusinessUnitDetailModel>
    {
        public Guid Id { get; set; }
    }
}
