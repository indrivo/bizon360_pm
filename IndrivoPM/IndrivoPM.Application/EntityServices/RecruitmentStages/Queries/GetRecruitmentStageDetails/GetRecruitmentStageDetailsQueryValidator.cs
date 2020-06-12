using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageDetails
{
    public class GetRecruitmentStageDetailsQueryValidator : AbstractValidator<GetRecruitmentStageDetailsQuery>
    {
        public GetRecruitmentStageDetailsQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty().WithMessage("The Id field is required");
        }
    }
}
