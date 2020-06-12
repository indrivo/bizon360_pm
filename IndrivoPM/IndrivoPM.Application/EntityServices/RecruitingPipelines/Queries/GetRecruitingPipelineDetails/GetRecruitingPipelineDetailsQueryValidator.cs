using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Queries.GetRecruitingPipelineDetails
{
    public class GetRecruitingPipelineDetailsQueryValidator : AbstractValidator<GetRecruitingPipelineDetailsQuery>
    {
        public GetRecruitingPipelineDetailsQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty().WithMessage("Error: Pipeline's Id missed"); // Todo: Remove msg, is never used.
        }
    }
}
