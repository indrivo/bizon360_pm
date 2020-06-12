using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageListByPipelineAndStatus
{
    public class GetRecruitmentStageListByPipelineAndStatusQueryValidator : AbstractValidator<GetRecruitmentStageListByPipelineAndStatusQuery>
    {
        public GetRecruitmentStageListByPipelineAndStatusQueryValidator()
        {
            RuleFor(a => a.RecruitingPipelineId)
                .NotEmpty()
                .NotNull();
        }
    }
}
