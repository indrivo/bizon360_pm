using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.CreateRecruitingPipeline
{
    public class CreateRecruitingPipelineCommandValidator : AbstractValidator<CreateRecruitingPipelineCommand>
    {
        public CreateRecruitingPipelineCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(a => a.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}
