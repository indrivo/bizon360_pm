

using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToStage
{
    public class BulkMoveCandidateToStageCommandValidator : AbstractValidator<BulkMoveCandidateToStageCommand>
    {
        public BulkMoveCandidateToStageCommandValidator()
        {
            RuleFor(a => a.StageId)
                .NotNull()
                .NotEmpty().WithMessage("The recruitment stage's Id was missed");

            RuleFor(a => a.CandidatesId)
                .NotNull()
                .NotEmpty().WithMessage("No candidate was matched to recruitment stage");
        }
    }
}
