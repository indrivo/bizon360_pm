using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToStage
{
    public class MoveCandidateToStageCommandValidator : AbstractValidator<MoveCandidateToStageCommand>
    {
        public MoveCandidateToStageCommandValidator()
        {
            RuleFor(a => a.StageId)
                .NotNull()
                .NotEmpty().WithMessage("The recruitment stage's Id was missed");

            RuleFor(a => a.CandidateId)
                .NotNull()
                .NotEmpty().WithMessage("No candidate was matched to recruitment stage");
        }
    }
}
