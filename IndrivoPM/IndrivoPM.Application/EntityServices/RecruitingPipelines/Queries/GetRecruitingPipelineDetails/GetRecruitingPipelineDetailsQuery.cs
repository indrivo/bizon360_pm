using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineDetails
{
    public class GetRecruitingPipelineDetailsQuery : IRequest<RecruitingPipelineDetailsModel>
    {
        public Guid Id { get; set; }
    }
}
