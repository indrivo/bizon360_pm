﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageList;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineDetails
{
    public class RecruitingPipelineDetailsModel
    {
        // Todo: Remove display attribute, is never used
        public Guid Id { get; set; }

        [Display(Name = "Recruiting pipeline name", Prompt = "Ex. ")]
        public string Name { get; set; }

        [Display(Name = "Description", Prompt = "same description")]
        public string Description { get; set; }

        public IList<RecruitmentStageLookUpModel> StageList { get; set; }


        public static Expression<Func<RecruitmentPipeline, RecruitingPipelineDetailsModel>> Projection
        {
            get
            {
                return recruitingPipeline => new RecruitingPipelineDetailsModel
                {
                    Id = recruitingPipeline.Id,
                    Name = recruitingPipeline.Name,
                    StageList = recruitingPipeline.RecruitmentStages != null ?
                        recruitingPipeline.RecruitmentStages.OrderBy(x => x.Name)
                            .Select(x => RecruitmentStageLookUpModel.Create(x)).ToList()
                        : new List<RecruitmentStageLookUpModel>()
                };
            }
        }

        public static RecruitingPipelineDetailsModel Create(RecruitmentPipeline pipeline)
        {
            return Projection.Compile().Invoke(pipeline);
        }
    }
}
