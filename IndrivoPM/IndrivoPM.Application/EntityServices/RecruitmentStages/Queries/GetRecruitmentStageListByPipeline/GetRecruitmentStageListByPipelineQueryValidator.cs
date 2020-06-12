using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipeline
{
    public class GetRecruitmentStageListbyPipelineQueryValidator : AbstractValidator<GetRecruitmentStageListByPipelineQuery>
    {
        public GetRecruitmentStageListbyPipelineQueryValidator()
        {
            RuleFor(a => a.RecruitingPipelineId)
                .NotEmpty()
                .NotNull();
        }
    }
}
