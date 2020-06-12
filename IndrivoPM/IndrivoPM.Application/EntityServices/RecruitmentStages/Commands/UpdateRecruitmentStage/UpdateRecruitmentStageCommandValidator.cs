using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.UpdateRecruitmentStage
{
    public class UpdateRecruitmentStageCommandValidator : AbstractValidator<UpdateRecruitmentStageCommand>
    {
        public UpdateRecruitmentStageCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty().WithMessage("Recruitment stage's Id was missed");

            RuleFor(a => a.Name)
                .NotNull()
                .NotEmpty().WithMessage("The name for recruitment stage is required")
                .MaximumLength(50).WithMessage("The name of recruitment stage cannot exceed 50 characters");
        }
    }
}
