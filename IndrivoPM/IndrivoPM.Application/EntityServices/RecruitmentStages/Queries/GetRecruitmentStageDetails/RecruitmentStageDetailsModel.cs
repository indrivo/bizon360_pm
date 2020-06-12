using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageDetails
{
    public class RecruitmentStageDetailsModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid RecruitingPipelineId { get; set; }

        public IList<CandidateLookupModel> Candidates { get; set; }

        public static Expression<Func<RecruitmentStage, RecruitmentStageDetailsModel>> Projection
        {
            get
            {
                return recruitmentStage => new RecruitmentStageDetailsModel
                {
                    Id = recruitmentStage.Id,
                    Name = recruitmentStage.Name,
                    RecruitingPipelineId = recruitmentStage.Pipeline.Id,
                    Candidates = recruitmentStage.Candidates != null ?
                        recruitmentStage.Candidates.OrderBy(x => x.Name).Select(x => CandidateLookupModel.Create(x)).ToList()
                        : new List<CandidateLookupModel>()
                };
            }
        }

        public static RecruitmentStageDetailsModel Create(RecruitmentStage recruitmentStage)
        {
            return Projection.Compile().Invoke(recruitmentStage);
        }
    }
}
