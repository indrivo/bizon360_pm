using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionDetails
{
    public class GetJobPositionDetailQuery:IRequest<JobPositionDetailModel>
    {
        public Guid Id { get; set; }

    }
}
