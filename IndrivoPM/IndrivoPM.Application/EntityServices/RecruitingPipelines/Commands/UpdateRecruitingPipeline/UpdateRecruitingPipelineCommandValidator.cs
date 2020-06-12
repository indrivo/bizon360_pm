using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.UpdateRecruitingPipeline
{
    public class UpdateRecruitingPipelineCommandValidator : AbstractValidator<UpdateRecruitingPipelineCommand>
    {
        public UpdateRecruitingPipelineCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty().WithMessage("Recruiting pipeline's Id was missed");

            RuleFor(a => a.Name)
                .NotNull()
                .NotEmpty().WithMessage("The name for recruiting pipeline is required")
                .MaximumLength(50).WithMessage("The name of recruiting pipeline cannot exceed 50 characters");

            RuleFor(a => a.Description)
                .NotNull()
                .NotEmpty().WithMessage("The description of recruiting pipeline is required")
                .MaximumLength(150).WithMessage("The description of recruiting pipeline cannot exceed 150 characters");
        }
    }
}
