using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.RenameRecruitmentStage
{
    public class RenameRecruitmentStageCommandValidator : AbstractValidator<RenameRecruitmentStageCommand>
    {
        public RenameRecruitmentStageCommandValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty();

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("A recruitment stage must have a name.")
                .MaximumLength(50).WithMessage("Recruitment stage name cannot exceed 50 characters.");
        }
    }
}
