using System;
using System.Collections.Generic;
using Gear.Domain.HrmEntities.Enums;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipelineAndStatus
{
    public class GetRecruitmentStageListByPipelineAndStatusQuery : IRequest<RecruitmentStageListViewModel>
    {
        public Guid RecruitingPipelineId { get; set; }

        public List<CandidateStatus> CandidateStatuses { get; set; }
    }
}
;