using System;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipeline
{
    public class GetRecruitmentStageListByPipelineQuery : IRequest<RecruitmentStageListViewModel>
    {
        public Guid RecruitingPipelineId { get; set; }
    }
}
