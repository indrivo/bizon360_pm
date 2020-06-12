using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToEmployee
{
    public class BulkMoveCandidateToEmployeeCommandValidator : AbstractValidator<BulkMoveCandidateToEmployeeCommand>
    {
        public BulkMoveCandidateToEmployeeCommandValidator()
        {
            RuleFor(a => a.CandidatesId)
                .NotNull()
                .NotEmpty().WithMessage("No candidate was matched to become an employee");
        }
    }
}
