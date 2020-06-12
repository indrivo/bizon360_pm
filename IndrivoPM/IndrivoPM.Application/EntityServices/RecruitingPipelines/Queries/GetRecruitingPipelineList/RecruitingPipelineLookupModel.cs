using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineList
{
    // Todo: Remove all display names.
    public class RecruitingPipelineLookupModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Recruiting pipeline name", Prompt = "Ex. ")]
        public string Name { get; set; }

        [Display(Name = "Description", Prompt = "same description")]
        public string Description { get; set; }

        public IList<RecruitmentStageLookUpModel> StageList { get; set; }

        public static Expression<Func<RecruitmentPipeline, RecruitingPipelineLookupModel>> Projection
        {
            get
            {
                return recruitingPipeline => new RecruitingPipelineLookupModel
                {
                    Id = recruitingPipeline.Id,
                    Name = recruitingPipeline.Name,
                    Description = recruitingPipeline.Description,
                    // Todo: Get only needed fields in a dictionary 
                    StageList = recruitingPipeline.RecruitmentStages != null ?
                    recruitingPipeline.RecruitmentStages.OrderBy(x => x.Name)
                        .Select(x => RecruitmentStageLookUpModel.Create(x)).ToList()
                    : new List<RecruitmentStageLookUpModel>()
                }; 
            }
        }

        public static RecruitingPipelineLookupModel Create(RecruitmentPipeline pipeline)
        {
            return Projection.Compile().Invoke(pipeline);
        }
    }
}
