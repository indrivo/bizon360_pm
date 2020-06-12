using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.AddRecruitmentStage
{
    public class CreateRecruitmentStageCommandValidator : AbstractValidator<CreateRecruitmentStageCommand>
    {
        public CreateRecruitmentStageCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .NotEmpty().WithMessage("The name for recruiting pipeline is required")
                .MaximumLength(50).WithMessage("The name of recruiting pipeline cannot exceed 50 characters");

            RuleFor(a => a.RecruitingPipelineId)
                .NotNull()
                .NotEmpty().WithMessage("You can't create the recruitment stage without the recruiting pipeline");
        }
    }
}
