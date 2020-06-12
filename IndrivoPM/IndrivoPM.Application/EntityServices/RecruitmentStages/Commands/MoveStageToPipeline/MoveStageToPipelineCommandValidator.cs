using FluentValidation;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.MoveStageToPipeline
{
    public class MoveStageToPipelineCommandValidator : AbstractValidator<MoveStageToPipelineCommand>
    {
        public MoveStageToPipelineCommandValidator()
        {
            RuleFor(a => a.RecruitingPipelineId)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.StagesId)
                .NotNull()
                .NotEmpty();
        }
    }
}
