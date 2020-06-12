using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList
{
    public class RecruitmentStageLookUpModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid PipelineId { get; set; }

        public IList<CandidateLookupModel> Candidates { get; set; }

        public static Expression<Func<RecruitmentStage, RecruitmentStageLookUpModel>> Projection
        {
            get
            {
                return recruitmentStage => new RecruitmentStageLookUpModel
                {
                    Id = recruitmentStage.Id,
                    Name = recruitmentStage.Name,
                    PipelineId = recruitmentStage.Pipeline.Id,
                    Candidates = recruitmentStage.Candidates != null ?
                        recruitmentStage.Candidates.OrderBy(x => x.Name).Select(x => CandidateLookupModel.Create(x)).ToList() 
                        : new List<CandidateLookupModel>()
                };
            }
        }

        public static RecruitmentStageLookUpModel Create(RecruitmentStage recruitmentStage)
        {
            return Projection.Compile().Invoke(recruitmentStage);
        }
    }
}
