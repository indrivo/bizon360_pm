using System;
using System.Collections.Generic;
using Gear.Domain.HrmEntities.Enums;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateListByStage
{
    public class GetCandidateListByStageQuery : IRequest<CandidateListViewModel>
    {
        public IEnumerable<CandidateStatus> Statuses { get; set; }

        public Guid RecruitmentStageId { get; set; }

        public string SearchParam { get; set; }
    }
}
